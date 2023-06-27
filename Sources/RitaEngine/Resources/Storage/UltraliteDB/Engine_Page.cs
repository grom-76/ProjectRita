using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#pragma warning disable

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    public enum PageType { Empty = 0, Header = 1, Collection = 2, Index = 3, Data = 4, Extend = 5 }

    internal abstract class BasePage
    {
        #region Page Constants

        /// <summary>
        /// The size of each page in disk - 4096 is NTFS default
        /// </summary>
        public const int PAGE_SIZE = 4096;

        /// <summary>
        /// This size is used bytes in header pages 17 bytes (+8 reserved to future use) = 25 bytes
        /// </summary>
        public const int PAGE_HEADER_SIZE = 25;

        /// <summary>
        /// Bytes available to store data removing page header size - 4071 bytes
        /// </summary>
        public const int PAGE_AVAILABLE_BYTES = PAGE_SIZE - PAGE_HEADER_SIZE;

        #endregion

        /// <summary>
        /// Represent page number - start in 0 with HeaderPage [4 bytes]
        /// </summary>
        public uint PageID { get; set; }

        /// <summary>
        /// Indicate the page type [1 byte] - Must be implemented for each page type
        /// </summary>
        public abstract PageType PageType { get; }

        /// <summary>
        /// Represent the previous page. Used for page-sequences - MaxValue represent that has NO previous page [4 bytes]
        /// </summary>
        public uint PrevPageID { get; set; }

        /// <summary>
        /// Represent the next page. Used for page-sequences - MaxValue represent that has NO next page [4 bytes]
        /// </summary>
        public uint NextPageID { get; set; }

        /// <summary>
        /// Used for all pages to count items inside this page(bytes, nodes, blocks, ...) [2 bytes]
        /// Its Int32 but writes in UInt16
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Used to find a free page using only header search [used in FreeList] [2 bytes]
        /// Its Int32 but writes in UInt16
        /// Its updated when a page modify content length (add/remove items)
        /// </summary>
        public int FreeBytes { get; set; }

        /// <summary>
        /// Indicate that this page is dirty (was modified) and must persist when committed [not-persistable]
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// This is the data when read first from disk - used to journal operations (IDiskService only will use)
        /// </summary>
        public byte[] DiskData { get; set; }

        public BasePage(uint pageID)
        {
            this.PageID = pageID;
            this.PrevPageID = uint.MaxValue;
            this.NextPageID = uint.MaxValue;
            this.ItemCount = 0;
            this.FreeBytes = PAGE_AVAILABLE_BYTES;
            this.DiskData = new byte[0];
        }

        /// <summary>
        /// Returns a size of specified number of pages
        /// </summary>
        /// <param name="pageCount">The page count</param>
        public static long GetSizeOfPages(uint pageCount)
        {
            return checked((long)pageCount * BasePage.PAGE_SIZE);
        }

        /// <summary>
        /// Returns a size of specified number of pages
        /// </summary>
        /// <param name="pageCount">The page count</param>
        public static long GetSizeOfPages(int pageCount)
        {
            if (pageCount < 0) throw new ArgumentOutOfRangeException("pageCount", "Could not be less than 0.");

            return BasePage.GetSizeOfPages((uint)pageCount);
        }

        #region Read/Write page

        /// <summary>
        /// Create a new instance of page based on T type
        /// </summary>
        public static T CreateInstance<T>(uint pageID)
            where T : BasePage
        {
            var type = typeof(T);

            // casting using "as T" #90 / thanks @Skysper
            if (type == typeof(HeaderPage)) return new HeaderPage() as T;
            if (type == typeof(CollectionPage)) return new CollectionPage(pageID) as T;
            if (type == typeof(IndexPage)) return new IndexPage(pageID) as T;
            if (type == typeof(DataPage)) return new DataPage(pageID) as T;
            if (type == typeof(ExtendPage)) return new ExtendPage(pageID) as T;
            if (type == typeof(EmptyPage)) return new EmptyPage(pageID) as T;

            throw new Exception("Invalid base page type T");
        }

        /// <summary>
        /// Create a new instance of page based on PageType
        /// </summary>
        public static BasePage CreateInstance(uint pageID, PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Collection: return new CollectionPage(pageID);
                case PageType.Index: return new IndexPage(pageID);
                case PageType.Data: return new DataPage(pageID);
                case PageType.Extend: return new ExtendPage(pageID);
                case PageType.Empty: return new EmptyPage(pageID);
                // use Header as default, because header page will read fixed HEADER_INFO and validate file format (if is not valid datafile)
                default: return new HeaderPage();
            }
        }

        /// <summary>
        /// Read a page with correct instance page object. Checks for pageType
        /// </summary>
        public static BasePage ReadPage(byte[] buffer)
        {
            var reader = new ByteReader(buffer);

            var pageID = reader.ReadUInt32();
            var pageType = (PageType)reader.ReadByte();

            if (pageID == 0 && (byte)pageType > 5)
            {
                throw UltraLiteException.InvalidDatabase();
            }

            var page = CreateInstance(pageID, pageType);

            page.ReadHeader(reader);
            page.ReadContent(reader);

            page.DiskData = buffer;

            return page;
        }

        /// <summary>
        /// Write a page to byte array
        /// </summary>
        public byte[] WritePage()
        {
            var writer = new ByteWriter(BasePage.PAGE_SIZE);

            this.WriteHeader(writer);

            if (this.PageType != PageType.Empty)
            {
                this.WriteContent(writer);
            }

            // update data bytes
            this.DiskData = writer.Buffer;

            return writer.Buffer;
        }

        private void ReadHeader(ByteReader reader)
        {
            // first 5 bytes (pageID + pageType) was readed before class create
            // this.PageID
            // this.PageType

            this.PrevPageID = reader.ReadUInt32();
            this.NextPageID = reader.ReadUInt32();
            this.ItemCount = reader.ReadUInt16();
            this.FreeBytes = reader.ReadUInt16();
            reader.Skip(8); // reserved 8 bytes
        }

        private void WriteHeader(ByteWriter writer)
        {
            writer.Write(this.PageID);
            writer.Write((byte)this.PageType);

            writer.Write(this.PrevPageID);
            writer.Write(this.NextPageID);
            writer.Write((UInt16)this.ItemCount);
            writer.Write((UInt16)this.FreeBytes);
            writer.Skip(8); // reserved 8 bytes
        }

        protected abstract void ReadContent(ByteReader reader);

        protected abstract void WriteContent(ByteWriter writer);

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Represents the collection page AND a collection item, because CollectionPage represent a Collection (1 page = 1 collection). All collections pages are linked with Prev/Next links
    /// </summary>
    internal class CollectionPage : BasePage
    {
        /// <summary>
        /// Represent maximum bytes that all collections names can be used in header
        /// </summary>
        public const ushort MAX_COLLECTIONS_SIZE = 3000;

        public static Regex NamePattern = new Regex(@"^[\w-]{1,60}$", RegexOptions.Compiled);

        /// <summary>
        /// Page type = Collection
        /// </summary>
        public override PageType PageType { get { return PageType.Collection; } }

        /// <summary>
        /// Name of collection
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Get a reference for the free list data page - its private list per collection - each DataPage contains only data for 1 collection (no mixing)
        /// Must to be a Field to be used as parameter reference
        /// </summary>
        public uint FreeDataPageID;

        /// <summary>
        /// Get the number of documents inside this collection
        /// </summary>
        public long DocumentCount { get; set; }

        /// <summary>
        /// Get all indexes from this collection - includes non-used indexes
        /// </summary>
        public CollectionIndex[] Indexes { get; set; }

        /// <summary>
        /// Storage number sequence to be used in auto _id values
        /// </summary>
        public long Sequence { get; set; }

        public CollectionPage(uint pageID)
            : base(pageID)
        {
            this.FreeDataPageID = uint.MaxValue;
            this.DocumentCount = 0;
            this.ItemCount = 1; // fixed for CollectionPage
            this.FreeBytes = 0; // no free bytes on collection-page - only one collection per page
            this.Indexes = new CollectionIndex[CollectionIndex.INDEX_PER_COLLECTION];
            this.Sequence = 0;

            for (var i = 0; i < Indexes.Length; i++)
            {
                this.Indexes[i] = new CollectionIndex() { Page = this, Slot = i };
            }
        }

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
            this.CollectionName = reader.ReadString();
            this.DocumentCount = reader.ReadInt64();
            this.FreeDataPageID = reader.ReadUInt32();

            foreach (var index in this.Indexes)
            {
                var field = reader.ReadString();
                var eq = field.IndexOf('=');

                // Use same string to avoid change file defition
                if (eq > 0)
                {
                    index.Field = field.Substring(0, eq);
                }
                else
                {
                    index.Field = field;
                }

                index.Unique = reader.ReadBoolean();
                index.HeadNode = reader.ReadPageAddress();
                index.TailNode = reader.ReadPageAddress();
                index.FreeIndexPageID = reader.ReadUInt32();
            }

            // position on page-footer (avoid file structure change)
            reader.Position = BasePage.PAGE_SIZE - 8 - CollectionIndex.INDEX_PER_COLLECTION;

            foreach (var index in this.Indexes)
            {
                var maxLevel = reader.ReadByte();
                index.MaxLevel = maxLevel == 0 ? (byte)IndexNode.MAX_LEVEL_LENGTH : maxLevel;
            }

            this.Sequence = reader.ReadInt64();
        }

        protected override void WriteContent(ByteWriter writer)
        {
            writer.Write(this.CollectionName);
            writer.Write(this.DocumentCount);
            writer.Write(this.FreeDataPageID);

            foreach (var index in this.Indexes)
            {
                // write Field+Expression only if index are used
                if(index.Field.Length > 0)
                {
                    writer.Write(index.Field + "=" + "$."+index.Field);
                }
                else
                {
                    writer.Write("");
                }

                writer.Write(index.Unique);
                writer.Write(index.HeadNode);
                writer.Write(index.TailNode);
                writer.Write(index.FreeIndexPageID);
            }

            // position on page-footer (avoid file structure change)
            writer.Position = BasePage.PAGE_SIZE - 8 - CollectionIndex.INDEX_PER_COLLECTION;

            foreach (var index in this.Indexes)
            {
                writer.Write(index.MaxLevel);
            }

            writer.Write(this.Sequence);
        }

        #endregion

        #region Methods to work with index array

        /// <summary>
        /// Returns first free index slot to be used
        /// </summary>
        public CollectionIndex GetFreeIndex()
        {
            for (byte i = 0; i < this.Indexes.Length; i++)
            {
                if (this.Indexes[i].IsEmpty) return this.Indexes[i];
            }

            throw UltraLiteException.IndexLimitExceeded(this.CollectionName);
        }

        /// <summary>
        /// Get index from field name (index field name is case sensitive) - returns null if not found
        /// </summary>
        public CollectionIndex GetIndex(string field)
        {
            return this.Indexes.FirstOrDefault(x => x.Field == field);
        }

        /// <summary>
        /// Get primary key index (_id index)
        /// </summary>
        public CollectionIndex PK { get { return this.Indexes[0]; } }

        /// <summary>
        /// Returns all used indexes
        /// </summary>
        public IEnumerable<CollectionIndex> GetIndexes(bool includePK)
        {
            return this.Indexes.Where(x => x.IsEmpty == false && x.Slot >= (includePK ? 0 : 1));
        }

 
        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// The DataPage thats stores object data.
    /// </summary>
    internal class DataPage : BasePage
    {
        /// <summary>
        /// Page type = Extend
        /// </summary>
        public override PageType PageType { get { return PageType.Data; } }

        /// <summary>
        /// If a Data Page has less that free space, it's considered full page for new items. Can be used only for update (DataPage) ~ 50% PAGE_AVAILABLE_BYTES
        /// This value is used for minimize
        /// </summary>
        public const int DATA_RESERVED_BYTES = PAGE_AVAILABLE_BYTES / 2;

        /// <summary>
        /// Returns all data blocks - Each block has one object
        /// </summary>
        private Dictionary<ushort, DataBlock> _dataBlocks = new Dictionary<ushort, DataBlock>();

        public Dictionary<ushort, DataBlock> DataBlocks => _dataBlocks;

        public DataPage(uint pageID)
            : base(pageID)
        {
        }

        /// <summary>
        /// Get datablock from internal blocks collection
        /// </summary>
        public DataBlock GetBlock(ushort index)
        {
            return _dataBlocks[index];
        }

        /// <summary>
        /// Add new data block into this page, update counter + free space
        /// </summary>
        public void AddBlock(DataBlock block)
        {
            var index = _dataBlocks.NextIndex();

            block.Position = new PageAddress(this.PageID, index);

            this.ItemCount++;
            this.FreeBytes -= block.Length;

            _dataBlocks.Add(index, block);
        }

        /// <summary>
        /// Update byte array from existing data block. Update free space too
        /// </summary>
        public void UpdateBlockData(DataBlock block, byte[] data)
        {
            this.FreeBytes = this.FreeBytes + block.Data.Length - data.Length;

            block.Data = data;
        }

        /// <summary>
        /// Remove data block from this page. Update counters and free space
        /// </summary>
        public void DeleteBlock(DataBlock block)
        {
            this.ItemCount--;
            this.FreeBytes += block.Length;

            _dataBlocks.Remove(block.Position.Index);
        }

        /// <summary>
        /// Get block counter from this page
        /// </summary>
        public int BlocksCount => _dataBlocks.Count;

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
            _dataBlocks = new Dictionary<ushort, DataBlock>(ItemCount);

            for (var i = 0; i < ItemCount; i++)
            {
                var block = new DataBlock();

                block.Page = this;
                block.Position = new PageAddress(this.PageID, reader.ReadUInt16());
                block.ExtendPageID = reader.ReadUInt32();
                var size = reader.ReadUInt16();
                block.Data = reader.ReadBytes(size);

                _dataBlocks.Add(block.Position.Index, block);
            }
        }

        protected override void WriteContent(ByteWriter writer)
        {
            foreach (var block in _dataBlocks.Values)
            {
                writer.Write(block.Position.Index);
                writer.Write(block.ExtendPageID);
                writer.Write((ushort)block.Data.Length);
                writer.Write(block.Data);
            }
        }

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Represent a empty page (reused)
    /// </summary>
    internal class EmptyPage : BasePage
    {
        /// <summary>
        /// Page type = Empty
        /// </summary>
        public override PageType PageType { get { return PageType.Empty; } }

        public EmptyPage(uint pageID)
            : base(pageID)
        {
            this.ItemCount = 0;
            this.FreeBytes = PAGE_AVAILABLE_BYTES;
        }

        public EmptyPage(BasePage page)
            : base(page.PageID)
        {
            if(page.DiskData.Length > 0)
            {
                this.DiskData = new byte[BasePage.PAGE_SIZE];
                Buffer.BlockCopy(page.DiskData, 0, this.DiskData, 0, BasePage.PAGE_SIZE);
            }
        }

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
        }

        protected override void WriteContent(ByteWriter writer)
        {
        }

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Represent a extra data page that contains the object when is not possible store in DataPage (bigger then  PAGE_SIZE or on update has no more space on page)
    /// Can be used in sequence of pages to store big objects
    /// </summary>
    internal class ExtendPage : BasePage
    {
        /// <summary>
        /// Page type = Extend
        /// </summary>
        public override PageType PageType { get { return PageType.Extend; } }

        /// <summary>
        /// Represent the part or full of the object - if this page has NextPageID the object is bigger than this page
        /// </summary>
        private byte[] _data = new byte[0];

        public ExtendPage(uint pageID)
            : base(pageID)
        {
        }

        /// <summary>
        /// Set slice of byte array source  into this page area
        /// </summary>
        public void SetData(byte[] data, int offset, int length)
        {
            this.ItemCount = length;
            this.FreeBytes = PAGE_AVAILABLE_BYTES - length; // not used on ExtendPage

            _data = new byte[length];

            Buffer.BlockCopy(data, offset, _data, 0, length);
        }

        /// <summary>
        /// Get internal page byte array data
        /// </summary>
        public byte[] GetData()
        {
            return _data;
        }

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
            _data = reader.ReadBytes(this.ItemCount);
        }

        protected override void WriteContent(ByteWriter writer)
        {
            writer.Write(_data);
        }

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    internal class HeaderPage : BasePage
    {
        /// <summary>
        /// Page type = Header
        /// </summary>
        public override PageType PageType { get { return PageType.Header; } }

        /// <summary>
        /// Header info the validate that datafile is a LiteDB file (27 bytes)
        /// </summary>
        private const string HEADER_INFO = "** This is a LiteDB file **";

        /// <summary>
        /// Datafile specification version
        /// </summary>
        private const byte FILE_VERSION = 7;

        /// <summary>
        /// Last modified transaction. Used to detect when other process change datafile and cache are not valid anymore
        /// </summary>
        public ushort ChangeID { get; set; }

        /// <summary>
        /// Get/Set the pageID that start sequence with a complete empty pages (can be used as a new page)
        /// Must be a field to be used as "ref"
        /// </summary>
        public uint FreeEmptyPageID;

        /// <summary>
        /// Last created page - Used when there is no free page inside file
        /// </summary>
        public uint LastPageID { get; set; }

        /// <summary>
        /// Database user version [2 bytes]
        /// </summary>
        public ushort UserVersion { get; set; }

        /// <summary>
        /// Password hash in SHA1 [20 bytes]
        /// </summary>
        public byte[] Password { get; set; }

        /// <summary>
        /// When using encryption, store salt for password
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// Indicate if datafile need be recovered
        /// </summary>
        public bool Recovery { get; set; }

        /// <summary>
        /// Get a dictionary with all collection pages with pageID link
        /// </summary>
        public Dictionary<string, uint> CollectionPages { get; set; }

        public HeaderPage()
            : base(0)
        {
            this.ChangeID = 0;
            this.FreeEmptyPageID = uint.MaxValue;
            this.LastPageID = 0;
            this.ItemCount = 1; // fixed for header
            this.FreeBytes = 0; // no free bytes on header
            this.UserVersion = 0;
            this.Password = new byte[20];
            this.Salt = new byte[16];
            this.CollectionPages = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
        }

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
            var info = reader.ReadString(HEADER_INFO.Length);
            var ver = reader.ReadByte();

            if (info != HEADER_INFO) throw UltraLiteException.InvalidDatabase();
            if (ver != FILE_VERSION) throw UltraLiteException.InvalidDatabaseVersion(ver);

            this.ChangeID = reader.ReadUInt16();
            this.FreeEmptyPageID = reader.ReadUInt32();
            this.LastPageID = reader.ReadUInt32();
            this.UserVersion = reader.ReadUInt16();
            this.Password = reader.ReadBytes(this.Password.Length);
            this.Salt = reader.ReadBytes(this.Salt.Length);

            // read page collections references (position on end of page)
            var cols = reader.ReadByte();
            for (var i = 0; i < cols; i++)
            {
                this.CollectionPages.Add(reader.ReadString(), reader.ReadUInt32());
            }

            // use last page byte position for recovery mode only because i forgot to reserve area before collection names!
            // TODO: fix this in next change data structure
            reader.Position = BasePage.PAGE_SIZE - 1;
            this.Recovery = reader.ReadBoolean();
        }

        protected override void WriteContent(ByteWriter writer)
        {
            writer.Write(HEADER_INFO, HEADER_INFO.Length);
            writer.Write(FILE_VERSION);
            writer.Write(this.ChangeID);
            writer.Write(this.FreeEmptyPageID);
            writer.Write(this.LastPageID);
            writer.Write(this.UserVersion);
            writer.Write(this.Password);
            writer.Write(this.Salt);

            writer.Write((byte)this.CollectionPages.Count);
            foreach (var key in this.CollectionPages.Keys)
            {
                writer.Write(key);
                writer.Write(this.CollectionPages[key]);
            }

            writer.Position = BasePage.PAGE_SIZE - 1;
            writer.Write(this.Recovery);
        }

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    internal class IndexPage : BasePage
    {
        /// <summary>
        /// Page type = Index
        /// </summary>
        public override PageType PageType { get { return PageType.Index; } }

        /// <summary>
        /// If a Index Page has less that this free space, it's considered full page for new items.
        /// </summary>
        public const int INDEX_RESERVED_BYTES = 100;

        private Dictionary<ushort, IndexNode> _nodes = new Dictionary<ushort, IndexNode>();

        public Dictionary<ushort, IndexNode> Nodes => _nodes;

        public IndexPage(uint pageID)
            : base(pageID)
        {
        }

        /// <summary>
        /// Get an index node from this page
        /// </summary>
        public IndexNode GetNode(ushort index)
        {
            return _nodes[index];
        }

        /// <summary>
        /// Add new index node into this page. Update counters and free space
        /// </summary>
        public void AddNode(IndexNode node)
        {
            var index = _nodes.NextIndex();

            node.Position = new PageAddress(this.PageID, index);

            this.ItemCount++;
            this.FreeBytes -= node.Length;

            _nodes.Add(index, node);
        }

        /// <summary>
        /// Delete node from this page and update counter and free space
        /// </summary>
        public void DeleteNode(IndexNode node)
        {
            this.ItemCount--;
            this.FreeBytes += node.Length;

            _nodes.Remove(node.Position.Index);
        }

        /// <summary>
        /// Get node counter
        /// </summary>
        public int NodesCount => _nodes.Count;

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
            _nodes = new Dictionary<ushort, IndexNode>(this.ItemCount);

            for (var i = 0; i < this.ItemCount; i++)
            {
                var index = reader.ReadUInt16();
                var levels = reader.ReadByte();

                var node = new IndexNode(levels);

                node.Page = this;
                node.Position = new PageAddress(this.PageID, index);
                node.Slot = reader.ReadByte();
                node.PrevNode = reader.ReadPageAddress();
                node.NextNode = reader.ReadPageAddress();
                node.KeyLength = reader.ReadUInt16();
                node.Key = reader.ReadBsonValue(node.KeyLength);
                node.DataBlock = reader.ReadPageAddress();

                for (var j = 0; j < node.Prev.Length; j++)
                {
                    node.Prev[j] = reader.ReadPageAddress();
                    node.Next[j] = reader.ReadPageAddress();
                }

                _nodes.Add(node.Position.Index, node);
            }
        }

        protected override void WriteContent(ByteWriter writer)
        {
            foreach (var node in _nodes.Values)
            {
                writer.Write(node.Position.Index); // node Index on this page
                writer.Write((byte)node.Prev.Length); // level length
                writer.Write(node.Slot); // index slot
                writer.Write(node.PrevNode); // prev node list
                writer.Write(node.NextNode); // next node list
                writer.Write(node.KeyLength); // valueLength
                writer.WriteBsonValue(node.Key, node.KeyLength); // value
                writer.Write(node.DataBlock); // data block reference

                for (var j = 0; j < node.Prev.Length; j++)
                {
                    writer.Write(node.Prev[j]);
                    writer.Write(node.Next[j]);
                }
            }
        }

        #endregion
    }
}

#pragma warning restore
