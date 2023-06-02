using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

#pragma warning disable

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    internal class CacheService
    {
        /// <summary>
        /// Collection to store clean only pages in cache
        /// </summary>
        private Dictionary<uint, BasePage> _clean = new Dictionary<uint, BasePage>();

        /// <summary>
        /// Collection to store dirty only pages in cache. If page was in _clean, remove from there and insert here
        /// </summary>
        private Dictionary<uint, BasePage> _dirty = new Dictionary<uint, BasePage>();

        private IDiskService _disk;
        private Logger _log;

        public CacheService(IDiskService disk, Logger log)
        {
            _disk = disk;
            _log = log;
        }

        /// <summary>
        /// Get a page from cache or from disk (and put on cache)
        /// [ThreadSafe]
        /// </summary>
        public BasePage GetPage(uint pageID)
        {
            // try get page from dirty cache or from clean list
            var page =
                _dirty.GetOrDefault(pageID) ??
                _clean.GetOrDefault(pageID);

            return page;
        }

        /// <summary>
        /// Add page to cache
        /// [ThreadSafe]
        /// </summary>
        public void AddPage(BasePage page)
        {
            if (page.IsDirty) throw new NotSupportedException("Page can't be dirty");

            _clean[page.PageID] = page;
        }

        /// <summary>
        /// Set a page as dirty and ensure page are in cache. Should be used after any change on page 
        /// Do not use on end of method because page can be deleted/change type
        /// Always remove from clean list and add in dirty list
        /// [ThreadSafe]
        /// </summary>
        public void SetDirty(BasePage page)
        {
            _clean.Remove(page.PageID);
            page.IsDirty = true;
            _dirty[page.PageID] = page;
        }

        /// <summary>
        /// Return all dirty pages
        /// [ThreadSafe]
        /// </summary>
        public ICollection<BasePage> GetDirtyPages()
        {
            return _dirty.Values;
        }

        /// <summary>
        /// Get how many pages are in clean cache
        /// </summary>
        public int CleanUsed { get { return _clean.Count; } }

        /// <summary>
        /// Get how many pages are in dirty cache
        /// </summary>
        public int DirtyUsed { get { return _dirty.Count; } }

        /// <summary>
        /// Discard only dirty pages
        /// [ThreadSafe]
        /// </summary>
        public void DiscardDirtyPages()
        {
            _log.Write(Logger.CACHE, "clearing dirty pages from cache");

            _dirty.Clear();
        }

        /// <summary>
        /// Mark all dirty pages now as clean pages and transfer to clean collection
        /// [ThreadSafe]
        /// </summary>
        public void MarkDirtyAsClean()
        {
            foreach(var p in _dirty)
            {
                p.Value.IsDirty = false;
                _clean[p.Key] = p.Value;
            }

            _dirty.Clear();
        }

        /// <summary>
        /// Remove from cache all clean pages
        /// [Non - ThreadSafe]
        /// </summary>
        public void ClearPages()
        {
            lock(_clean)
            {
                _log.Write(Logger.CACHE, "cleaning cache");
                _clean.Clear();
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    internal class CollectionService
    {
        private PageService _pager;
        private IndexService _indexer;
        private DataService _data;
        private TransactionService _trans;
        private Logger _log;

        public CollectionService(PageService pager, IndexService indexer, DataService data, TransactionService trans, Logger log)
        {
            _pager = pager;
            _indexer = indexer;
            _data = data;
            _trans = trans;
            _log = log;
        }

        /// <summary>
        /// Get a exist collection. Returns null if not exists
        /// </summary>
        public CollectionPage Get(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var header = _pager.GetPage<HeaderPage>(0);

            uint pageID;

            if (header.CollectionPages.TryGetValue(name, out pageID))
            {
                return _pager.GetPage<CollectionPage>(pageID);
            }

            return null;
        }

        /// <summary>
        /// Add a new collection. Check if name the not exists
        /// </summary>
        public CollectionPage Add(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (!CollectionPage.NamePattern.IsMatch(name)) throw UltraLiteException.InvalidFormat(name);

            _log.Write(Logger.COMMAND, "creating new collection '{0}'", name);

            // get header marked as dirty because I will use header after (and NewPage can get another header instance)
            var header = _pager.GetPage<HeaderPage>(0);

            // check limit count (8 bytes per collection = 4 to string length, 4 for uint pageID)
            if (header.CollectionPages.Sum(x => x.Key.Length + 8) + name.Length + 8 >= CollectionPage.MAX_COLLECTIONS_SIZE)
            {
                throw UltraLiteException.CollectionLimitExceeded(CollectionPage.MAX_COLLECTIONS_SIZE);
            }

            // get new collection page (marked as dirty)
            var col = _pager.NewPage<CollectionPage>();

            // add this page to header page collection
            header.CollectionPages.Add(name, col.PageID);

            col.CollectionName = name;

            // set header page as dirty
            _pager.SetDirty(header);

            // create PK index
            var pk = _indexer.CreateIndex(col);

            pk.Field = "_id";
            pk.Unique = true;

            return col;
        }

        /// <summary>
        /// Get all collections pages
        /// </summary>
        public IEnumerable<CollectionPage> GetAll()
        {
            var header = _pager.GetPage<HeaderPage>(0);

            foreach (var pageID in header.CollectionPages.Values)
            {
                yield return _pager.GetPage<CollectionPage>(pageID);
            }
        }

        /// <summary>
        /// Drop a collection - remove all data pages + indexes pages
        /// </summary>
        public void Drop(CollectionPage col)
        {
            // add all pages to delete
            var pages = new HashSet<uint>();

            // search for all data page and index page
            foreach (var index in col.GetIndexes(true))
            {
                // get all nodes from index
                var nodes = _indexer.FindAll(index, Query.Ascending);

                foreach (var node in nodes)
                {
                    // if is PK index, add dataPages
                    if (index.Slot == 0)
                    {
                        pages.Add(node.DataBlock.PageID);

                        // read datablock to check if there is any extended page
                        var block = _data.GetBlock(node.DataBlock);

                        if (block.ExtendPageID != uint.MaxValue)
                        {
                            _pager.DeletePage(block.ExtendPageID, true);
                        }
                    }

                    // memory checkpoint
                    _trans.CheckPoint();

                    // add index page to delete list page
                    pages.Add(node.Position.PageID);
                }

                // remove head+tail nodes in all indexes
                pages.Add(index.HeadNode.PageID);
                pages.Add(index.TailNode.PageID);
            }

            // and now, lets delete all this pages
            foreach (var pageID in pages)
            {
                // delete page
                _pager.DeletePage(pageID);

                // memory checkpoint
                _trans.CheckPoint();
            }

            // get header page to remove from collection list links
            var header = _pager.GetPage<HeaderPage>(0);

            header.CollectionPages.Remove(col.CollectionName);

            // set header as dirty after remove
            _pager.SetDirty(header);

            _pager.DeletePage(col.PageID);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    internal class DataService
    {
        private PageService _pager;
        private Logger _log;

        public DataService(PageService pager, Logger log)
        {
            _pager = pager;
            _log = log;
        }

        /// <summary>
        /// Insert data inside a datapage. Returns dataPageID that indicates the first page
        /// </summary>
        public DataBlock Insert(CollectionPage col, byte[] data)
        {
            // need to extend (data is bigger than 1 page)
            var extend = (data.Length + DataBlock.DATA_BLOCK_FIXED_SIZE) > BasePage.PAGE_AVAILABLE_BYTES;

            // if extend, just search for a page with BLOCK_SIZE available
            var dataPage = _pager.GetFreePage<DataPage>(col.FreeDataPageID, extend ? DataBlock.DATA_BLOCK_FIXED_SIZE : data.Length + DataBlock.DATA_BLOCK_FIXED_SIZE);

            // create a new block with first empty index on DataPage
            var block = new DataBlock { Page = dataPage };

            // if extend, store all bytes on extended page.
            if (extend)
            {
                var extendPage = _pager.NewPage<ExtendPage>();
                block.ExtendPageID = extendPage.PageID;
                this.StoreExtendData(extendPage, data);
            }
            else
            {
                block.Data = data;
            }

            // add dataBlock to this page
            dataPage.AddBlock(block);

            // set page as dirty
            _pager.SetDirty(dataPage);

            // add/remove dataPage on freelist if has space
            _pager.AddOrRemoveToFreeList(dataPage.FreeBytes > DataPage.DATA_RESERVED_BYTES, dataPage, col, ref col.FreeDataPageID);

            // increase document count in collection
            col.DocumentCount++;

            // set collection page as dirty
            _pager.SetDirty(col);

            return block;
        }

        /// <summary>
        /// Update data inside a datapage. If new data can be used in same datapage, just update. Otherwise, copy content to a new ExtendedPage
        /// </summary>
        public DataBlock Update(CollectionPage col, PageAddress blockAddress, byte[] data)
        {
            // get datapage and mark as dirty
            var dataPage = _pager.GetPage<DataPage>(blockAddress.PageID);
            var block = dataPage.GetBlock(blockAddress.Index);
            var extend = dataPage.FreeBytes + block.Data.Length - data.Length <= 0;

            // check if need to extend
            if (extend)
            {
                // clear my block data
                dataPage.UpdateBlockData(block, new byte[0]);

                // create (or get a existed) extendpage and store data there
                ExtendPage extendPage;

                if (block.ExtendPageID == uint.MaxValue)
                {
                    extendPage = _pager.NewPage<ExtendPage>();
                    block.ExtendPageID = extendPage.PageID;
                }
                else
                {
                    extendPage = _pager.GetPage<ExtendPage>(block.ExtendPageID);
                }

                this.StoreExtendData(extendPage, data);
            }
            else
            {
                // if no extends, just update data block
                dataPage.UpdateBlockData(block, data);

                // if there was a extended bytes, delete
                if (block.ExtendPageID != uint.MaxValue)
                {
                    _pager.DeletePage(block.ExtendPageID, true);
                    block.ExtendPageID = uint.MaxValue;
                }
            }

            // set DataPage as dirty
            _pager.SetDirty(dataPage);

            // add/remove dataPage on freelist if has space AND its on/off free list
            _pager.AddOrRemoveToFreeList(dataPage.FreeBytes > DataPage.DATA_RESERVED_BYTES, dataPage, col, ref col.FreeDataPageID);

            return block;
        }

        /// <summary>
        /// Read all data from datafile using a pageID as reference. If data is not in DataPage, read from ExtendPage.
        /// </summary>
        public byte[] Read(PageAddress blockAddress)
        {
            var block = this.GetBlock(blockAddress);

            // if there is a extend page, read bytes all bytes from extended pages
            if (block.ExtendPageID != uint.MaxValue)
            {
                return this.ReadExtendData(block.ExtendPageID);
            }

            return block.Data;
        }

        /// <summary>
        /// Get a data block from a DataPage using address
        /// </summary>
        public DataBlock GetBlock(PageAddress blockAddress)
        {
            var page = _pager.GetPage<DataPage>(blockAddress.PageID);
            return page.GetBlock(blockAddress.Index);
        }

        /// <summary>
        /// Read all data from a extended page with all subsequences pages if exits
        /// </summary>
        public byte[] ReadExtendData(uint extendPageID)
        {
            // read all extended pages and build byte array
            using (var buffer = new MemoryStream())
            {
                foreach (var extendPage in _pager.GetSeqPages<ExtendPage>(extendPageID))
                {
                    buffer.Write(extendPage.GetData(), 0, extendPage.ItemCount);
                }

                return buffer.ToArray();
            }
        }

        /// <summary>
        /// Delete one dataBlock
        /// </summary>
        public DataBlock Delete(CollectionPage col, PageAddress blockAddress)
        {
            // get page and mark as dirty
            var page = _pager.GetPage<DataPage>(blockAddress.PageID);
            var block = page.GetBlock(blockAddress.Index);

            // if there a extended page, delete all
            if (block.ExtendPageID != uint.MaxValue)
            {
                _pager.DeletePage(block.ExtendPageID, true);
            }

            // delete block inside page
            page.DeleteBlock(block);

            // set page as dirty here
            _pager.SetDirty(page);

            // if there is no more datablocks, lets delete all page
            if (page.BlocksCount == 0)
            {
                // first, remove from free list
                _pager.AddOrRemoveToFreeList(false, page, col, ref col.FreeDataPageID);

                _pager.DeletePage(page.PageID);
            }
            else
            {
                // add or remove to free list
                _pager.AddOrRemoveToFreeList(page.FreeBytes > DataPage.DATA_RESERVED_BYTES, page, col, ref col.FreeDataPageID);
            }

            col.DocumentCount--;

            // mark collection page as dirty
            _pager.SetDirty(col);

            return block;
        }

        /// <summary>
        /// Store all bytes in one extended page. If data ir bigger than a page, store in more pages and make all in sequence
        /// </summary>
        public void StoreExtendData(ExtendPage page, byte[] data)
        {
            var offset = 0;
            var bytesLeft = data.Length;

            while (bytesLeft > 0)
            {
                var bytesToCopy = System.Math.Min(bytesLeft, BasePage.PAGE_AVAILABLE_BYTES);

                page.SetData(data, offset, bytesToCopy);

                bytesLeft -= bytesToCopy;
                offset += bytesToCopy;

                // set extend page as dirty
                _pager.SetDirty(page);

                // if has bytes left, let's get a new page
                if (bytesLeft > 0)
                {
                    // if i have a continuous page, get it... or create a new one
                    page = page.NextPageID != uint.MaxValue ?
                        _pager.GetPage<ExtendPage>(page.NextPageID) :
                        _pager.NewPage<ExtendPage>(page);
                }
            }

            // when finish, check if last page has a nextPageId - if have, delete them
            if (page.NextPageID != uint.MaxValue)
            {
                // Delete nextpage and all nexts
                _pager.DeletePage(page.NextPageID, true);

                // set my page with no NextPageID
                page.NextPageID = uint.MaxValue;

                // set page as dirty
                _pager.SetDirty(page);
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Implement a Index service - Add/Remove index nodes on SkipList
    /// Based on: http://igoro.com/archive/skip-lists-are-fascinating/
    /// </summary>
    internal class IndexService
    {
        /// <summary>
        /// Max size of a index entry - usde for string, binary, array and documents
        /// </summary>
        public const int MAX_INDEX_LENGTH = 512;

        private PageService _pager;
        private Logger _log;
        private Random _rand = new Random();

        public IndexService(PageService pager, Logger log)
        {
            _pager = pager;
            _log = log;
        }

        /// <summary>
        /// Create a new index and returns head page address (skip list)
        /// </summary>
        public CollectionIndex CreateIndex(CollectionPage col)
        {
            // get index slot
            var index = col.GetFreeIndex();

            // get a new index page for first index page
            var page = _pager.NewPage<IndexPage>();

            // create a empty node with full max level
            var head = new IndexNode(IndexNode.MAX_LEVEL_LENGTH)
            {
                Key = BsonValue.MinValue,
                KeyLength = (ushort)BsonValue.MinValue.GetBytesCount(false),
                Slot = (byte)index.Slot,
                Page = page
            };

            // add as first node
            page.AddNode(head);

            // set index page as dirty
            _pager.SetDirty(index.Page);

            // add indexPage on freelist if has space
            _pager.AddOrRemoveToFreeList(true, page, index.Page, ref index.FreeIndexPageID);

            // point the head/tail node to this new node position
            index.HeadNode = head.Position;

            // insert tail node
            var tail = this.AddNode(index, BsonValue.MaxValue, IndexNode.MAX_LEVEL_LENGTH, null);

            index.TailNode = tail.Position;

            return index;
        }

        /// <summary>
        /// Insert a new node index inside an collection index. Flip coin to know level
        /// </summary>
        public IndexNode AddNode(CollectionIndex index, BsonValue key, IndexNode last)
        {
            var level = this.FlipCoin();

            // set index collection with max-index level
            if (level > index.MaxLevel)
            {
                index.MaxLevel = level;

                _pager.SetDirty(index.Page);
            }

            // call AddNode with key value
            return this.AddNode(index, key, level, last);
        }

        /// <summary>
        /// Insert a new node index inside an collection index.
        /// </summary>
        private IndexNode AddNode(CollectionIndex index, BsonValue key, byte level, IndexNode last)
        {
            // calc key size
            var keyLength = key.GetBytesCount(false);

            // test for index key maxlength
            if (keyLength > MAX_INDEX_LENGTH) throw UltraLiteException.IndexKeyTooLong();

            // creating a new index node
            var node = new IndexNode(level)
            {
                Key = key,
                KeyLength = (ushort)keyLength,
                Slot = (byte)index.Slot
            };

            // get a free page to insert my index node
            var page = _pager.GetFreePage<IndexPage>(index.FreeIndexPageID, node.Length);

            node.Page = page;

            // add index node to page
            page.AddNode(node);

            // now, let's link my index node on right place
            var cur = this.GetNode(index.HeadNode);

            // using as cache last
            IndexNode cache = null;

            // scan from top left
            for (var i = index.MaxLevel - 1; i >= 0; i--)
            {
                // get cache for last node
                cache = cache != null && cache.Position.Equals(cur.Next[i]) ? cache : this.GetNode(cur.Next[i]);

                // for(; <while_not_this>; <do_this>) { ... }
                for (; cur.Next[i].IsEmpty == false; cur = cache)
                {
                    // get cache for last node
                    cache = cache != null && cache.Position.Equals(cur.Next[i]) ? cache : this.GetNode(cur.Next[i]);

                    // read next node to compare
                    var diff = cache.Key.CompareTo(key);

                    // if unique and diff = 0, throw index exception (must rollback transaction - others nodes can be dirty)
                    if (diff == 0 && index.Unique) throw UltraLiteException.IndexDuplicateKey(index.Field, key);

                    if (diff == 1) break;
                }

                if (i <= (level - 1)) // level == length
                {
                    // cur = current (immediately before - prev)
                    // node = new inserted node
                    // next = next node (where cur is pointing)
                    _pager.SetDirty(cur.Page);

                    node.Next[i] = cur.Next[i];
                    node.Prev[i] = cur.Position;
                    cur.Next[i] = node.Position;

                    var next = this.GetNode(node.Next[i]);

                    if (next != null)
                    {
                        next.Prev[i] = node.Position;
                        _pager.SetDirty(next.Page);
                    }
                }
            }

            // add/remove indexPage on freelist if has space
            _pager.AddOrRemoveToFreeList(page.FreeBytes > IndexPage.INDEX_RESERVED_BYTES, page, index.Page, ref index.FreeIndexPageID);

            // if last node exists, create a double link list
            if (last != null)
            {
                // link new node with last node
                if (last.NextNode.IsEmpty == false)
                {
                    // fix link pointer with has more nodes in list
                    var next = this.GetNode(last.NextNode);
                    next.PrevNode = node.Position;
                    last.NextNode = node.Position;
                    node.PrevNode = last.Position;
                    node.NextNode = next.Position;

                    _pager.SetDirty(next.Page);
                }
                else
                {
                    last.NextNode = node.Position;
                    node.PrevNode = last.Position;
                }

                // set last node page as dirty
                _pager.SetDirty(last.Page);
            }

            return node;
        }

        /// <summary>
        /// Gets all node list from any index node (go forward and backward)
        /// </summary>
        public IEnumerable<IndexNode> GetNodeList(IndexNode node, bool includeInitial)
        {
            var next = node.NextNode;
            var prev = node.PrevNode;

            // returns some initial node
            if (includeInitial) yield return node;

            // go forward
            while (next.IsEmpty == false)
            {
                var n = this.GetNode(next);
                next = n.NextNode;
                yield return n;
            }

            // go backward
            while (prev.IsEmpty == false)
            {
                var p = this.GetNode(prev);
                prev = p.PrevNode;
                yield return p;
            }
        }

        /// <summary>
        /// Deletes an indexNode from a Index and adjust Next/Prev nodes
        /// </summary>
        public void Delete(CollectionIndex index, PageAddress nodeAddress)
        {
            var node = this.GetNode(nodeAddress);
            var page = node.Page;

            // mark page as dirty here because, if deleted, page type will change
            _pager.SetDirty(page);

            for (int i = node.Prev.Length - 1; i >= 0; i--)
            {
                // get previous and next nodes (between my deleted node)
                var prev = this.GetNode(node.Prev[i]);
                var next = this.GetNode(node.Next[i]);

                if (prev != null)
                {
                    prev.Next[i] = node.Next[i];
                    _pager.SetDirty(prev.Page);
                }
                if (next != null)
                {
                    next.Prev[i] = node.Prev[i];
                    _pager.SetDirty(next.Page);
                }
            }

            page.DeleteNode(node);

            // if there is no more nodes in this page, delete them
            if (page.NodesCount == 0)
            {
                // first, remove from free list
                _pager.AddOrRemoveToFreeList(false, page, index.Page, ref index.FreeIndexPageID);

                _pager.DeletePage(page.PageID);
            }
            else
            {
                // add or remove page from free list
                _pager.AddOrRemoveToFreeList(page.FreeBytes > IndexPage.INDEX_RESERVED_BYTES, node.Page, index.Page, ref index.FreeIndexPageID);
            }

            // now remove node from nodelist 
            var prevNode = this.GetNode(node.PrevNode);
            var nextNode = this.GetNode(node.NextNode);

            if (prevNode != null)
            {
                prevNode.NextNode = node.NextNode;
                _pager.SetDirty(prevNode.Page);
            }
            if (nextNode != null)
            {
                nextNode.PrevNode = node.PrevNode;
                _pager.SetDirty(nextNode.Page);
            }
        }

        /// <summary>
        /// Drop all indexes pages. Each index use a single page sequence
        /// </summary>
        public void DropIndex(CollectionIndex index)
        {
            var pages = new HashSet<uint>();
            var nodes = this.FindAll(index, Query.Ascending);

            // get reference for pageID from all index nodes
            foreach (var node in nodes)
            {
                pages.Add(node.Position.PageID);

                // for each node I need remove from node list datablock reference
                var prevNode = this.GetNode(node.PrevNode);
                var nextNode = this.GetNode(node.NextNode);

                if (prevNode != null)
                {
                    prevNode.NextNode = node.NextNode;
                    _pager.SetDirty(prevNode.Page);
                }
                if (nextNode != null)
                {
                    nextNode.PrevNode = node.PrevNode;
                    _pager.SetDirty(nextNode.Page);
                }
            }

            // now delete all pages
            foreach (var pageID in pages)
            {
                _pager.DeletePage(pageID);
            }
        }

        /// <summary>
        /// Get a node inside a page using PageAddress - Returns null if address IsEmpty
        /// </summary>
        public IndexNode GetNode(PageAddress address)
        {
            if (address.IsEmpty) return null;
            var page = _pager.GetPage<IndexPage>(address.PageID);
            return page.GetNode(address.Index);
        }

        /// <summary>
        /// Flip coin - skip list - returns level node (start in 1)
        /// </summary>
        public byte FlipCoin()
        {
            byte level = 1;
            for (int R = _rand.Next(); (R & 1) == 1; R >>= 1)
            {
                level++;
                if (level == IndexNode.MAX_LEVEL_LENGTH) break;
            }
            return level;
        }

        #region Find

        public IEnumerable<IndexNode> FindAll(CollectionIndex index, int order)
        {
            var cur = this.GetNode(order == Query.Ascending ? index.HeadNode : index.TailNode);

            while (!cur.NextPrev(0, order).IsEmpty)
            {
                cur = this.GetNode(cur.NextPrev(0, order));

                // stop if node is head/tail
                if (cur.IsHeadTail(index)) yield break;

                yield return cur;
            }
        }

        /// <summary>
        /// Find first node that index match with value. If not found but sibling = true, returns near node (only non-unique index)
        /// Before find, value must be normalized
        /// </summary>
        public IndexNode Find(CollectionIndex index, BsonValue value, bool sibling, int order)
        {
            var cur = this.GetNode(order == Query.Ascending ? index.HeadNode : index.TailNode);

            for (var i = index.MaxLevel - 1; i >= 0; i--)
            {
                for (; cur.NextPrev(i, order).IsEmpty == false; cur = this.GetNode(cur.NextPrev(i, order)))
                {
                    var next = this.GetNode(cur.NextPrev(i, order));
                    var diff = next.Key.CompareTo(value);

                    if (diff == order && (i > 0 || !sibling)) break;
                    if (diff == order && i == 0 && sibling)
                    {
                        return next.IsHeadTail(index) ? null : next;
                    }

                    // if equals, test for duplicates - go back to first occurs on duplicate values
                    if (diff == 0)
                    {
                        // if unique index has no duplicates - just return node
                        if (index.Unique) return next;

                        return this.FindBoundary(index, next, value, order * -1, i);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Goto the first/last occurrence of this index value
        /// </summary>
        private IndexNode FindBoundary(CollectionIndex index, IndexNode cur, BsonValue value, int order, int level)
        {
            var last = cur;

            while (cur.Key.CompareTo(value) == 0)
            {
                last = cur;
                cur = this.GetNode(cur.NextPrev(0, order));
                if (cur.IsHeadTail(index)) break;
            }

            return last;
        }

        #endregion
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	internal class PageService
    {
        private CacheService _cache;
        private IDiskService _disk;
        private AesEncryption _crypto;
        private Logger _log;

        public PageService(IDiskService disk, AesEncryption crypto, CacheService cache, Logger log)
        {
            _disk = disk;
            _crypto = crypto;
            _cache = cache;
            _log = log;
        }

        /// <summary>
        /// Get a page from cache or from disk (get from cache or from disk)
        /// </summary>
        public T GetPage<T>(uint pageID)
            where T : BasePage
        {
            lock(_disk)
            {
                var page = _cache.GetPage(pageID);

                // is not on cache? load from disk
                if (page == null)
                {
                    var buffer = _disk.ReadPage(pageID);

                    // if datafile are encrypted, decrypt buffer (header are not encrypted)
                    if (_crypto != null && pageID > 0)
                    {
                        buffer = _crypto.Decrypt(buffer);
                    }

                    page = BasePage.ReadPage(buffer);

                    _cache.AddPage(page);
                }

                return (T)page;
            }
        }

        /// <summary>
        /// Set a page as dirty and ensure page are in cache. Should be used after any change on page 
        /// Do not use on end of method because page can be deleted/change type
        /// </summary>
        public void SetDirty(BasePage page)
        {
            _cache.SetDirty(page);
        }

        /// <summary>
        /// Read all sequences pages from a start pageID (using NextPageID)
        /// </summary>
        public IEnumerable<T> GetSeqPages<T>(uint firstPageID)
            where T : BasePage
        {
            var pageID = firstPageID;

            while (pageID != uint.MaxValue)
            {
                var page = this.GetPage<T>(pageID);

                pageID = page.NextPageID;

                yield return page;
            }
        }

        /// <summary>
        /// Get a new empty page - can be a reused page (EmptyPage) or a clean one (extend datafile)
        /// </summary>
        public T NewPage<T>(BasePage prevPage = null)
            where T : BasePage
        {
            // get header
            var header = this.GetPage<HeaderPage>(0);
            var pageID = (uint)0;
            var diskData = new byte[0];

            // try get page from Empty free list
            if (header.FreeEmptyPageID != uint.MaxValue)
            {
                var free = this.GetPage<BasePage>(header.FreeEmptyPageID);

                // remove page from empty list
                this.AddOrRemoveToFreeList(false, free, header, ref header.FreeEmptyPageID);

                pageID = free.PageID;

                // if used page has original disk data, copy to my new page
                if (free.DiskData.Length > 0)
                {
                    diskData = free.DiskData;
                }
            }
            else
            {
                pageID = ++header.LastPageID;

                // set header page as dirty after increment LastPageID
                this.SetDirty(header);
            }

            var page = BasePage.CreateInstance<T>(pageID);

            // copy disk data from re-used page (or be an empty)
            page.DiskData = diskData;

            // add page to cache with correct T type (could be an old Empty page type)
            this.SetDirty(page);

            // if there a page before, just fix NextPageID pointer
            if (prevPage != null)
            {
                page.PrevPageID = prevPage.PageID;
                prevPage.NextPageID = page.PageID;

                this.SetDirty(prevPage);
            }

            return page;
        }

        /// <summary>
        /// Delete an page using pageID - transform them in Empty Page and add to EmptyPageList
        /// If you delete a page, you can using same old instance of page - page will be converter to EmptyPage
        /// If need deleted page, use GetPage again
        /// </summary>
        public void DeletePage(uint pageID, bool addSequence = false)
        {
            // get all pages in sequence or a single one
            var pages = addSequence ? this.GetSeqPages<BasePage>(pageID).ToArray() : new BasePage[] { this.GetPage<BasePage>(pageID) };

            // get my header page
            var header = this.GetPage<HeaderPage>(0);

            // adding all pages to FreeList
            foreach (var page in pages)
            {
                // create a new empty page based on a normal page
                var empty = new EmptyPage(page.PageID);

                // add empty page to cache (with now EmptyPage type) and mark as dirty
                this.SetDirty(empty);

                // add to empty free list
                this.AddOrRemoveToFreeList(true, empty, header, ref header.FreeEmptyPageID);
            }
        }

        /// <summary>
        /// Returns a page that contains space enough to data to insert new object - if one does not exit, creates a new page.
        /// </summary>
        public T GetFreePage<T>(uint startPageID, int size)
            where T : BasePage
        {
            if (startPageID != uint.MaxValue)
            {
                // get the first page
                var page = this.GetPage<T>(startPageID);

                // check if there space in this page
                var free = page.FreeBytes;

                // first, test if there is space on this page
                if (free >= size)
                {
                    return page;
                }
            }

            // if not has space on first page, there is no page with space (pages are ordered), create a new one
            return this.NewPage<T>();
        }

        #region Add Or Remove do empty list

        /// <summary>
        /// Add or Remove a page in a sequence
        /// </summary>
        /// <param name="add">Indicate that will add or remove from FreeList</param>
        /// <param name="page">Page to add or remove from FreeList</param>
        /// <param name="startPage">Page reference where start the header list node</param>
        /// <param name="fieldPageID">Field reference, from startPage</param>
        public void AddOrRemoveToFreeList(bool add, BasePage page, BasePage startPage, ref uint fieldPageID)
        {
            if (add)
            {
                // if page has no prev/next it's not on list - lets add
                if (page.PrevPageID == uint.MaxValue && page.NextPageID == uint.MaxValue)
                {
                    this.AddToFreeList(page, startPage, ref fieldPageID);
                }
                else
                {
                    // otherwise this page is already in this list, lets move do put in free size desc order
                    this.MoveToFreeList(page, startPage, ref fieldPageID);
                }
            }
            else
            {
                // if this page is not in sequence, its not on freelist
                if (page.PrevPageID == uint.MaxValue && page.NextPageID == uint.MaxValue)
                    return;

                this.RemoveToFreeList(page, startPage, ref fieldPageID);
            }
        }

        /// <summary>
        /// Add a page in free list in desc free size order
        /// </summary>
        private void AddToFreeList(BasePage page, BasePage startPage, ref uint fieldPageID)
        {
            var free = page.FreeBytes;
            var nextPageID = fieldPageID;
            BasePage next = null;

            // let's page in desc order
            while (nextPageID != uint.MaxValue)
            {
                next = this.GetPage<BasePage>(nextPageID);

                if (free >= next.FreeBytes)
                {
                    // assume my page in place of next page
                    page.PrevPageID = next.PrevPageID;
                    page.NextPageID = next.PageID;

                    // link next page to my page
                    next.PrevPageID = page.PageID;

                    // mark next page as dirty
                    this.SetDirty(next);
                    this.SetDirty(page);

                    // my page is the new first page on list
                    if (page.PrevPageID == 0)
                    {
                        fieldPageID = page.PageID;
                        this.SetDirty(startPage); // fieldPageID is from startPage
                    }
                    else
                    {
                        // if not the first, ajust links from previous page (set as dirty)
                        var prev = this.GetPage<BasePage>(page.PrevPageID);
                        prev.NextPageID = page.PageID;
                        this.SetDirty(prev);
                    }

                    return; // job done - exit
                }

                nextPageID = next.NextPageID;
            }

            // empty list, be the first
            if (next == null)
            {
                // it's first page on list
                page.PrevPageID = 0;
                fieldPageID = page.PageID;

                this.SetDirty(startPage);
            }
            else
            {
                // it's last position on list (next = last page on list)
                page.PrevPageID = next.PageID;
                next.NextPageID = page.PageID;

                this.SetDirty(next);
            }

            // set current page as dirty
            this.SetDirty(page);
        }

        /// <summary>
        /// Remove a page from list - the ease part
        /// </summary>
        private void RemoveToFreeList(BasePage page, BasePage startPage, ref uint fieldPageID)
        {
            // this page is the first of list
            if (page.PrevPageID == 0)
            {
                fieldPageID = page.NextPageID;
                this.SetDirty(startPage); // fieldPageID is from startPage
            }
            else
            {
                // if not the first, get previous page to remove NextPageId
                var prevPage = this.GetPage<BasePage>(page.PrevPageID);
                prevPage.NextPageID = page.NextPageID;
                this.SetDirty(prevPage);
            }

            // if my page is not the last on sequence, adjust the last page (set as dirty)
            if (page.NextPageID != uint.MaxValue)
            {
                var nextPage = this.GetPage<BasePage>(page.NextPageID);
                nextPage.PrevPageID = page.PrevPageID;
                this.SetDirty(nextPage);
            }

            page.PrevPageID = page.NextPageID = uint.MaxValue;

            // mark page that will be removed as dirty
            this.SetDirty(page);
        }

        /// <summary>
        /// When a page is already on a list it's more efficient just move comparing with siblings
        /// </summary>
        private void MoveToFreeList(BasePage page, BasePage startPage, ref uint fieldPageID)
        {
            //TODO: write a better solution
            this.RemoveToFreeList(page, startPage, ref fieldPageID);
            this.AddToFreeList(page, startPage, ref fieldPageID);
        }

        #endregion
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Manages all transactions and grantees concurrency and recovery
    /// </summary>
    internal class TransactionService
    {
        private IDiskService _disk;
        private AesEncryption _crypto;
        private PageService _pager;
        private CacheService _cache;
        private Logger _log;
        private int _cacheSize;

        internal TransactionService(IDiskService disk, AesEncryption crypto, PageService pager, CacheService cache, int cacheSize, Logger log)
        {
            _disk = disk;
            _crypto = crypto;
            _cache = cache;
            _pager = pager;
            _cacheSize = cacheSize;
            _log = log;
        }

        /// <summary>
        /// Checkpoint is a safe point to clear cache pages without loose pages references.
        /// Is called after each document insert/update/deleted/indexed/fetch from query
        /// Clear only clean pages - do not clear dirty pages (transaction)
        /// Return true if cache was clear
        /// </summary>
        public bool CheckPoint()
        {
            if (_cache.CleanUsed > _cacheSize)
            {
                _log.Write(Logger.CACHE, "cache size reached {0} pages, will clear now", _cache.CleanUsed);

                _cache.ClearPages();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Save all dirty pages to disk
        /// </summary>
        public void PersistDirtyPages()
        {
            // get header page
            var header = _pager.GetPage<HeaderPage>(0);

            // increase file changeID (back to 0 when overflow)
            header.ChangeID = header.ChangeID == ushort.MaxValue ? (ushort)0 : (ushort)(header.ChangeID + (ushort)1);

            // mark header as dirty
            _pager.SetDirty(header);

            _log.Write(Logger.DISK, "begin disk operations - changeID: {0}", header.ChangeID);

            // write journal file in desc order to header be last page in disk
            if (_disk.IsJournalEnabled)
            {
                _disk.WriteJournal(_cache.GetDirtyPages()
                    .OrderByDescending(x => x.PageID)
                    .Select(x => x.DiskData)
                    .Where(x => x.Length > 0)
                    .ToList(), header.LastPageID);

                // mark header as recovery before start writing (in journal, must keep recovery = false)
                header.Recovery = true;

                // flush to disk to ensure journal is committed to disk before proceeding
                _disk.Flush();
            }
            else
            {
                // if no journal extend, resize file here to fast writes
                _disk.SetLength(BasePage.GetSizeOfPages(header.LastPageID + 1));
            }

            // write header page first. if header.Recovery == true, this ensures it's written to disk *before* we start changing pages
            var headerPage = _cache.GetPage(0);
            var headerBuffer = headerPage.WritePage();
            _disk.WritePage(0, headerBuffer);
            _disk.Flush();

            // get all dirty page stating from Header page (SortedList)
            // header page (id=0) always must be first page to write on disk because it's will mark disk as "in recovery"
            foreach (var page in _cache.GetDirtyPages())
            {
                // we've already written the header, so skip it
                if (page.PageID == 0)
                {
                    continue;
                }

                // page.WritePage() updated DiskData with new rendered buffer
                var buffer = _crypto == null || page.PageID == 0 ? 
                    page.WritePage() : 
                    _crypto.Encrypt(page.WritePage());

                _disk.WritePage(page.PageID, buffer);
            }

            if (_disk.IsJournalEnabled)
            {
                // ensure changed pages are persisted to disk
                _disk.Flush();

                // re-write header page but now with recovery=false
                header.Recovery = false;

                _log.Write(Logger.DISK, "re-write header page now with recovery = false");

                _disk.WritePage(0, header.WritePage());
            }

            // mark all dirty pages as clean pages (all are persisted in disk and are valid pages)
            _cache.MarkDirtyAsClean();

            // flush all data direct to disk
            _disk.Flush();

            // discard journal file
            _disk.ClearJournal(header.LastPageID);
        }

        /// <summary>
        /// Get journal pages and override all into datafile
        /// </summary>
        public void Recovery()
        {
            _log.Write(Logger.RECOVERY, "initializing recovery mode");


            // double check in header need recovery (could be already recover from another thread)
            var header = BasePage.ReadPage(_disk.ReadPage(0)) as HeaderPage;

            if (header.Recovery == false) return;

            // read all journal pages
            foreach (var buffer in _disk.ReadJournal(header.LastPageID))
            {
                // read pageID (first 4 bytes)
                var pageID = BitConverter.ToUInt32(buffer, 0);

                _log.Write(Logger.RECOVERY, "recover page #{0:0000}", pageID);

                // write in stream (encrypt if datafile is encrypted)
                _disk.WritePage(pageID, _crypto == null || pageID == 0 ? buffer : _crypto.Encrypt(buffer));
            }

            // shrink datafile
            _disk.ClearJournal(header.LastPageID);
            
        }
    }
}

#pragma warning restore