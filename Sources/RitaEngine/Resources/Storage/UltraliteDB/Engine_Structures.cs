using System.Text.RegularExpressions;
using System.Collections.Generic;

#pragma warning disable

#region  Engine_Structures

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
	internal class CollectionIndex
    {
        public static Regex IndexPattern = new Regex(@"^[\w](\.?[\w\$][\w-]*){0,59}$", RegexOptions.Compiled);

        /// <summary>
        /// Total indexes per collection - it's fixed because I will used fixed arrays allocations
        /// </summary>
        public const int INDEX_PER_COLLECTION = 16;

        /// <summary>
        /// Represent slot position on index array on dataBlock/collection indexes - non-persistable
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Field name
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Indicate if this index has distinct values only
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// Head page address for this index
        /// </summary>
        public PageAddress HeadNode { get; set; }

        /// <summary>
        /// A link pointer to tail node
        /// </summary>
        public PageAddress TailNode { get; set; }

        /// <summary>
        /// Get a reference for the free list index page - its private list per collection/index (must be a Field to be used as reference parameter)
        /// </summary>
        public uint FreeIndexPageID;

        /// <summary>
        /// Returns if this index slot is empty and can be used as new index
        /// </summary>
        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(this.Field); }
        }

        /// <summary>
        /// Persist max level used
        /// </summary>
        public byte MaxLevel { get; set; }

        /// <summary>
        /// Get a reference for page
        /// </summary>
        public CollectionPage Page { get; set; }

        public CollectionIndex()
        {
            this.Clear();
        }

        /// <summary>
        /// Clear all index information
        /// </summary>
        public void Clear()
        {
            this.Field = string.Empty;
            this.Unique = false;
            this.HeadNode = PageAddress.Empty;
            this.FreeIndexPageID = uint.MaxValue;
            this.MaxLevel = 1;
        }
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
	internal class DataBlock
    {
        public const int DATA_BLOCK_FIXED_SIZE = 2 + // Position.Index (ushort)
                                                 4 + // ExtendedPageID (uint)
                                                 2; // block.Data.Length (ushort)

        /// <summary>
        /// Position of this dataBlock inside a page (store only Position.Index)
        /// </summary>
        public PageAddress Position { get; set; }

        /// <summary>
        /// If object is bigger than this page - use a ExtendPage (and do not use Data array)
        /// </summary>
        public uint ExtendPageID { get; set; }

        /// <summary>
        /// Data of a record - could be empty if is used in ExtedPage
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Get a reference for page
        /// </summary>
        public DataPage Page { get; set; }

        /// <summary>
        /// Get length of this dataBlock (persist as ushort 2 bytes)
        /// </summary>
        public int Length
        {
            get { return DataBlock.DATA_BLOCK_FIXED_SIZE + this.Data.Length; }
        }

        public DataBlock()
        {
            this.Position = PageAddress.Empty;
            this.ExtendPageID = uint.MaxValue;
            this.Data = new byte[0];
        }
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Represent a index information
	/// </summary>
	public class IndexInfo
    {
        internal IndexInfo(CollectionIndex index)
        {
            this.Slot = index.Slot;
            this.Field = index.Field;
            this.Unique = index.Unique;
            this.MaxLevel = index.MaxLevel;
        }

        /// <summary>
        /// Slot number of index
        /// </summary>
        public int Slot { get; private set; }

        /// <summary>
        /// Field index name
        /// </summary>
        public string Field { get; private set; }

        /// <summary>
        /// Index is Unique?
        /// </summary>
        public bool Unique { get; private set; }

        /// <summary>
        /// Indicate max level used in skip-list
        /// </summary>
        public byte MaxLevel { get; private set; }
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Represent a index node inside a Index Page
	/// </summary>
	internal class IndexNode
    {
        public const int INDEX_NODE_FIXED_SIZE = 2 + // Position.Index (ushort)
                                                 1 + // Levels (byte)
                                                 2 + // ValueLength (ushort)
                                                 1 + // BsonType (byte)
                                                 1 + // Slot (1 byte)
                                                 (PageAddress.SIZE * 2) + // Prev/Next Node (6 bytes)
                                                 PageAddress.SIZE; // DataBlock

        /// <summary>
        /// Max level used on skip list
        /// </summary>
        public const int MAX_LEVEL_LENGTH = 32;

        /// <summary>
        /// Position of this node inside a IndexPage - Store only Position.Index
        /// </summary>
        public PageAddress Position { get; set; }

        /// <summary>
        /// Slot position of index in data block
        /// </summary>
        public byte Slot { get; set; }

        /// <summary>
        /// Prev node in same document list index nodes
        /// </summary>
        public PageAddress PrevNode { get; set; }

        /// <summary>
        /// Next node in same document list index nodes
        /// </summary>
        public PageAddress NextNode { get; set; }

        /// <summary>
        /// Link to prev value (used in skip lists - Prev.Length = Next.Length)
        /// </summary>
        public PageAddress[] Prev { get; set; }

        /// <summary>
        /// Link to next value (used in skip lists - Prev.Length = Next.Length)
        /// </summary>
        public PageAddress[] Next { get; set; }

        /// <summary>
        /// Length of key - used for calculate Node size
        /// </summary>
        public ushort KeyLength { get; set; }

        /// <summary>
        /// The object value that was indexed
        /// </summary>
        public BsonValue Key { get; set; }

        /// <summary>
        /// Reference for a datablock - the value
        /// </summary>
        public PageAddress DataBlock { get; set; }

        /// <summary>
        /// Get page reference
        /// </summary>
        public IndexPage Page { get; set; }

        /// <summary>
        /// Returns Next (order == 1) OR Prev (order == -1)
        /// </summary>
        public PageAddress NextPrev(int index, int order)
        {
            return order == Query.Ascending ? this.Next[index] : this.Prev[index];
        }

        /// <summary>
        /// Returns if this node is header or tail from collection Index
        /// </summary>
        public bool IsHeadTail(CollectionIndex index)
        {
            return this.Position.Equals(index.HeadNode) || this.Position.Equals(index.TailNode);
        }

        /// <summary>
        /// Get the length size of this node in disk - not persistable
        /// </summary>
        public int Length
        {
            get
            {
                return IndexNode.INDEX_NODE_FIXED_SIZE +
                    (this.Prev.Length * PageAddress.SIZE * 2) + // Prev + Next
                    this.KeyLength; // bytes count in BsonValue
            }
        }

        /// <summary>
        /// Cached document - if null, use DataBlock
        /// </summary>
        public BsonDocument CacheDocument { get; set; }

        public IndexNode(byte level)
        {
            this.Position = PageAddress.Empty;
            this.PrevNode = PageAddress.Empty;
            this.NextNode = PageAddress.Empty;
            this.DataBlock = PageAddress.Empty;
            this.Prev = new PageAddress[level];
            this.Next = new PageAddress[level];

            for (var i = 0; i < level; i++)
            {
                this.Prev[i] = PageAddress.Empty;
                this.Next[i] = PageAddress.Empty;
            }
        }
    }

    internal class IndexNodeComparer : IEqualityComparer<IndexNode>
    {
        public bool Equals(IndexNode x, IndexNode y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            if (x == null || y == null) return false;

            return x.DataBlock.Equals(y.DataBlock);
        }

        public int GetHashCode(IndexNode obj)
        {
            return obj.DataBlock.GetHashCode();
        }
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Represents a page address inside a page structure - index could be byte offset position OR index in a list (6 bytes)
	/// </summary>
	internal struct PageAddress
    {
        public const int SIZE = 6;

        public static PageAddress Empty = new PageAddress(uint.MaxValue, ushort.MaxValue);

        /// <summary>
        /// PageID (4 bytes)
        /// </summary>
        public uint PageID;

        /// <summary>
        /// Index inside page (2 bytes)
        /// </summary>
        public ushort Index;

        public bool IsEmpty
        {
            get { return PageID == uint.MaxValue; }
        }

        public override bool Equals(object obj)
        {
            var other = (PageAddress)obj;

            return this.PageID == other.PageID && this.Index == other.Index;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                // Maybe nullity checks, if these are objects not primitives!
                hash = hash * 23 + (int)this.PageID;
                hash = hash * 23 + this.Index;
                return hash;
            }
        }

        public PageAddress(uint pageID, ushort index)
        {
            PageID = pageID;
            Index = index;
        }

        public override string ToString()
        {
            return IsEmpty ? "----" : PageID.ToString() + ":" + Index.ToString();
        }
    }
}

#endregion

#pragma warning restore
