using System;
using System.Collections.Generic;
using System.Linq;

#region ENGINE QUERY 

#pragma warning disable

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Class helper to create query using indexes in database. All methods are statics.
    /// Queries can be executed in 3 ways: Index Seek (fast), Index Scan (good), Full Scan (slow)
    /// </summary>
    public abstract class Query
    {
        public string Field { get; private set; }

        internal BsonFields Expression { get; set; }
        internal virtual bool UseIndex { get; set; }
        internal virtual bool UseFilter { get; set; }

        internal Query(string field)
        {
            this.Field = field;
        }

        #region Static Methods

        /// <summary>
        /// Indicate when a query must execute in ascending order
        /// </summary>
        public const int Ascending = 1;

        /// <summary>
        /// Indicate when a query must execute in descending order
        /// </summary>
        public const int Descending = -1;

        /// <summary>
        /// Returns all documents using _id index order
        /// </summary>
        public static Query All(int order = Ascending)
        {
            return new QueryAll("_id", order);
        }

        /// <summary>
        /// Returns all documents using field index order
        /// </summary>
        public static Query All(string field, int order = Ascending)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            return new QueryAll(field, order);
        }

        /// <summary>
        /// Returns all documents that value are equals to value (=)
        /// </summary>
        public static Query EQ(string field, BsonValue value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            return new QueryEquals(field, value ?? BsonValue.Null);
        }

        /// <summary>
        /// Returns all documents that value are less than value (&lt;)
        /// </summary>
        public static Query LT(string field, BsonValue value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            return new QueryLess(field, value ?? BsonValue.Null, false);
        }

        /// <summary>
        /// Returns all documents that value are less than or equals value (&lt;=)
        /// </summary>
        public static Query LTE(string field, BsonValue value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            return new QueryLess(field, value ?? BsonValue.Null, true);
        }

        /// <summary>
        /// Returns all document that value are greater than value (&gt;)
        /// </summary>
        public static Query GT(string field, BsonValue value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            return new QueryGreater(field, value ?? BsonValue.Null, false);
        }

        /// <summary>
        /// Returns all documents that value are greater than or equals value (&gt;=)
        /// </summary>
        public static Query GTE(string field, BsonValue value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            return new QueryGreater(field, value ?? BsonValue.Null, true);
        }

        /// <summary>
        /// Returns all document that values are between "start" and "end" values (BETWEEN)
        /// </summary>
        public static Query Between(string field, BsonValue start, BsonValue end, bool startEquals = true, bool endEquals = true)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            return new QueryBetween(field, start ?? BsonValue.Null, end ?? BsonValue.Null, startEquals, endEquals);
        }

        /// <summary>
        /// Returns all documents that starts with value (LIKE)
        /// </summary>
        public static Query StartsWith(string field, string value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));
            if (value.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(value));

            return new QueryStartsWith(field, value);
        }

        /// <summary>
        /// Returns all documents that contains value (CONTAINS)
        /// </summary>
        public static Query Contains(string field, string value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));
            if (value.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(value));

            return new QueryContains(field, value);
        }

        /// <summary>
        /// Returns all documents that are not equals to value (not equals)
        /// </summary>
        public static Query Not(string field, BsonValue value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));
            return new QueryNotEquals(field, value ?? BsonValue.Null);
        }

        /// <summary>
        /// Returns all documents that in query result (not result)
        /// </summary>
        public static Query Not(Query query, int order = Query.Ascending)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return new QueryNot(query, order);
        }

        /// <summary>
        /// Returns all documents that has value in values list (IN)
        /// </summary>
        public static Query In(string field, BsonArray value)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new QueryIn(field, value.RawValue);
        }

        /// <summary>
        /// Returns all documents that has value in values list (IN)
        /// </summary>
        public static Query In(string field, params BsonValue[] values)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));
            if (values == null) throw new ArgumentNullException(nameof(values));

            return new QueryIn(field, values);
        }

        /// <summary>
        /// Returns all documents that has value in values list (IN)
        /// </summary>
        public static Query In(string field, IEnumerable<BsonValue> values)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));
            if (values == null) throw new ArgumentNullException(nameof(values));

            return new QueryIn(field, values);
        }

        /// <summary>
        /// Apply a predicate function in an index result. Execute full index scan but it's faster then runs over deserialized document.
        /// </summary>
        public static Query Where(string field, Func<BsonValue, bool> predicate, int order = Query.Ascending)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return new QueryWhere(field, predicate, order);
        }

        /// <summary>
        /// Returns document that exists in BOTH queries results. If both queries has indexes, left query has index preference (other side will be run in full scan)
        /// </summary>
        public static Query And(Query left, Query right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            // test if can use QueryBetween because it's more efficient
            if (left is QueryGreater && right is QueryLess && left.Field == right.Field)
            {
                var l = left as QueryGreater;
                var r = right as QueryLess;

                return Between(l.Field, l.Value, r.Value, l.IsEquals, r.IsEquals);
            }

            return new QueryAnd(left, right);
        }

        /// <summary>
        /// Returns document that exists in ALL queries results.
        /// </summary>
        public static Query And(params Query[] queries)
        {
            if (queries == null || queries.Length < 2) throw new ArgumentException("At least two Query should be passed");

            var left = queries[0];

            for (int i = 1; i < queries.Length; i++)
            {
                left = And(left, queries[i]);
            }
            return left;
        }

        /// <summary>
        /// Returns documents that exists in ANY queries results (Union).
        /// </summary>
        public static Query Or(Query left, Query right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new QueryOr(left, right);
        }

        /// <summary>
        /// Returns document that exists in ANY queries results (Union).
        /// </summary>
        public static Query Or(params Query[] queries)
        {
            if (queries == null || queries.Length < 2) throw new ArgumentException("At least two Query should be passed");

            var left = queries[0];

            for (int i = 1; i < queries.Length; i++)
            {
                left = Or(left, queries[i]);
            }
            return left;
        }

        #endregion

        #region Executing Query

        /// <summary>
        /// Find witch index will be used and run Execute method
        /// </summary>
        internal virtual IEnumerable<IndexNode> Run(CollectionPage col, IndexService indexer)
        {
            // get index for this query
            var index = col.GetIndex(this.Field);

            // if index not found, must use Filter (full scan)
            if (index == null)
            {
                this.UseFilter = true;

                // create expression based on Field
                this.Expression = new BsonFields(this.Field);

                // returns all index nodes - (will use Filter method later)
                return indexer.FindAll(col.PK, Query.Ascending);
            }
            else
            {
                this.UseIndex = true;

                this.Expression = new BsonFields(index.Field);

                // execute query to get all IndexNodes
                // do DistinctBy datablock to not duplicate same document in results
                return this.ExecuteIndex(indexer, index)
                    .DistinctBy(x => x.DataBlock, null);
            }
        }

        /// <summary>
        /// Abstract method that must be implement for index seek/scan - Returns IndexNodes that match with index
        /// </summary>
        internal abstract IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index);

        /// <summary>
        /// Abstract method that must implement full scan - will be called for each document and need
        /// returns true if condition was satisfied
        /// </summary>
        internal abstract bool FilterDocument(BsonDocument doc);

        #endregion
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// All is an Index Scan operation
    /// </summary>
    internal class QueryAll : Query
    {
        private int _order;

        public QueryAll(string field, int order)
            : base(field)
        {
            _order = order;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            return indexer.FindAll(index, _order);
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})",
                this.UseFilter ? "Filter" : this.UseIndex ? "Scan" : "",
                this.Field);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    internal class QueryAnd : Query
    {
        private Query _left;
        private Query _right;

        public QueryAnd(Query left, Query right)
            : base(null)
        {
            _left = left;
            _right = right;
        }

        internal override bool UseFilter
        {
            get
            {
                // return true if any site use filter
                return _left.UseFilter || _right.UseFilter;
            }
            set
            {
                // set both sides with value
                _left.UseFilter = value;
                _right.UseFilter = value;
            }
        }

        internal override IEnumerable<IndexNode> Run(CollectionPage col, IndexService indexer)
        {
            // execute both run operation but not fetch any node yet
            var left = _left.Run(col, indexer);
            var right = _right.Run(col, indexer);

            // if left use index, force right use full scan (left has preference to use index)
            if (_left.UseIndex)
            {
                this.UseIndex = true;
                _right.UseFilter = true;
                return left;
            }

            // if right use index (and left no), force left use filter
            if (_right.UseIndex)
            {
                this.UseIndex = true;
                _left.UseFilter = true;
                return right;
            }

            // neither left and right uses index (both are full scan)
            this.UseIndex = false;
            this.UseFilter = true;

            return left.Intersect(right, new IndexNodeComparer());
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            throw new NotSupportedException();
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return _left.FilterDocument(doc) && _right.FilterDocument(doc);
        }

        public override string ToString()
        {
            return string.Format("({0} and {1})", _left, _right);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    internal class QueryBetween : Query
    {
        private BsonValue _start;
        private BsonValue _end;

        private bool _startEquals;
        private bool _endEquals;

        public QueryBetween(string field, BsonValue start, BsonValue end, bool startEquals, bool endEquals)
            : base(field)
        {
            _start = start;
            _startEquals = startEquals;
            _end = end;
            _endEquals = endEquals;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            // define order
            var order = _start.CompareTo(_end) <= 0 ? Query.Ascending : Query.Descending;

            // find first indexNode
            var node = indexer.Find(index, _start, true, order);

            // returns (or not) equals start value
            while (node != null)
            {
                var diff = node.Key.CompareTo(_start);

                // if current value are not equals start, go out this loop
                if (diff != 0) break;

                if (_startEquals)
                {
                    yield return node;
                }

                node = indexer.GetNode(node.NextPrev(0, order));
            }

            // navigate using next[0] do next node - if less or equals returns
            while (node != null)
            {
                var diff = node.Key.CompareTo(_end);

                if (_endEquals && diff == 0)
                {
                    yield return node;
                }
                else if (diff == -order)
                {
                    yield return node;
                }
                else
                {
                    break;
                }

                node = indexer.GetNode(node.NextPrev(0, order));
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return this.Expression.Execute(doc, false)
                .Any(x =>
                {
                    return
                        (_startEquals ? x.CompareTo(_start) >= 0 : x.CompareTo(_start) > 0) &&
                        (_endEquals ? x.CompareTo(_end) <= 0 : x.CompareTo(_end) < 0);
                });
        }

        public override string ToString()
        {
            return string.Format("{0}({1} between {2}{3} and {4}{5})",
                this.UseFilter ? "Filter" : this.UseIndex ? "IndexSeek" : "",
                this.Field,
                _startEquals ? "[" : "(",
                _start, 
                _end,
                _endEquals ? "]" : ")"
                );
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Contains query do not work with index, only full scan
	/// </summary>
	internal class QueryContains : Query
    {
        private BsonValue _value;

        public QueryContains(string field, BsonValue value)
            : base(field)
        {
            _value = value;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            return indexer
                .FindAll(index, Query.Ascending)
                .Where(x => x.Key.IsString && x.Key.AsString.Contains(_value));
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return this.Expression.Execute(doc, false)
                .Where(x => x.IsString)
                .Any(x => x.AsString.Contains(_value));
        }

        public override string ToString()
        {
            return string.Format("{0}({1} contains {2})",
                this.UseFilter ? "Filter" : this.UseIndex ? "Scan" : "",
                this.Field,
                _value);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Include all components to be used in execution of a qery
	/// </summary>
	internal class QueryCursor : IDisposable
    {
        private int _position;
        private int _skip;
        private int _limit;
        private Query _query;
        private IEnumerator<IndexNode> _nodes;

        public List<BsonDocument> Documents { get; private set; }
        public bool HasMore { get; private set; }

        public QueryCursor(Query query, int skip, int limit)
        {
            _query = query;
            _skip = skip;
            _limit = limit;
            _position = skip;
            _nodes = null;

            this.HasMore = true;
            this.Documents = new List<BsonDocument>();
        }

        /// <summary>
        /// Initialize nodes enumeator with query execution
        /// </summary>
        public void Initialize(IEnumerator<IndexNode> nodes)
        {
            _nodes = nodes;
        }

        /// <summary>
        /// ReQuery result and set skip counter to current position
        /// </summary>
        public void ReQuery(IEnumerator<IndexNode> nodes)
        {
            _nodes = nodes;
            _skip = _position;
        }

        /// <summary>
        /// Fetch documents from enumerator and add to buffer. If cache recycle, stop read to execute in another read
        /// </summary>
        public void Fetch(TransactionService trans, DataService data)
        {
            // empty document buffer
            this.Documents.Clear();

            // while until must cache not recycle
            while (trans.CheckPoint() == false)
            {
                // read next node
                this.HasMore = _nodes.MoveNext();

                // if finish, exit loop
                if (this.HasMore == false) return;

                // if run ONLY under index, skip/limit before deserialize
                if (_query.UseIndex && _query.UseFilter == false)
                {
                    if (--_skip >= 0) continue;

                    if (--_limit <= -1)
                    {
                        this.HasMore = false;
                        return;
                    }
                }

                // get current node
                var node = _nodes.Current;

                // read document from data block
                var buffer = data.Read(node.DataBlock);
                var doc = BsonReader.Deserialize(buffer).AsDocument;

                // if need run in full scan, execute full scan and test return
                if (_query.UseFilter)
                {
                    // execute query condition here - if false, do not add on final results
                    if (_query.FilterDocument(doc) == false) continue;

                    // implement skip/limit after deserialize in full scan
                    if (--_skip >= 0) continue;

                    if (--_limit <= -1)
                    {
                        this.HasMore = false;
                        return;
                    }
                }

                // increment position cursor
                _position++;

                // avoid lock again just to check limit
                if (_limit == 0)
                {
                    this.HasMore = false;
                }

                this.Documents.Add(doc);
            }
        }

        public void Dispose()
        {
            if (_nodes != null)
            {
                _nodes.Dispose();
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Placeholder query for returning no values from a collection.
	/// </summary>
	internal class QueryEmpty : Query
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryEmpty" /> class.
        /// </summary>
        public QueryEmpty()
            : base(null)
        {
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            yield break;
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("(false)");
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	internal class QueryEquals : Query
    {
        private BsonValue _value;

        public QueryEquals(string field, BsonValue value)
            : base(field)
        {
            _value = value;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            var node = indexer.Find(index, _value, false, Query.Ascending);

            if (node == null) yield break;

            yield return node;

            if (index.Unique == false)
            {
                // navigate using next[0] do next node - if equals, returns
                while (!node.Next[0].IsEmpty && ((node = indexer.GetNode(node.Next[0])).Key.CompareTo(_value) == 0))
                {
                    if (node.IsHeadTail(index)) yield break;

                    yield return node;
                }
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return this.Expression.Execute(doc, true)
                .Any(x => x.CompareTo(_value) == 0);
        }

        public override string ToString()
        {
            return string.Format("{0}({1} = {2})",
                this.UseFilter ? "Filter" : this.UseIndex ? "Seek" : "",
                this.Field,
                _value);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	internal class QueryGreater : Query
    {
        private BsonValue _value;
        private bool _equals;

        public BsonValue Value { get { return _value; } }
        public bool IsEquals { get { return _equals; } }

        public QueryGreater(string field, BsonValue value, bool equals)
            : base(field)
        {
            _value = value;
            _equals = equals;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            // find first indexNode
            var node = indexer.Find(index, _value, true, Query.Ascending);

            if (node == null) yield break;

            // move until next is last
            while (node != null)
            {
                // compares only with are same type
                if (node.Key.Type == _value.Type || (node.Key.IsNumber && _value.IsNumber))
                {
                    var diff = node.Key.CompareTo(_value);

                    if (diff == 1 || (_equals && diff == 0))
                    {
                        if (node.IsHeadTail(index)) yield break;

                        yield return node;
                    }
                }

                node = indexer.GetNode(node.Next[0]);
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return this.Expression.Execute(doc, true)
                .Where(x => x.Type == _value.Type || (x.IsNumber && _value.IsNumber))
                .Any(x => x.CompareTo(_value) >= (_equals ? 0 : 1));
        }

        public override string ToString()
        {
            return string.Format("{0}({1} >{2} {3})",
                this.UseFilter ? "Filter" : this.UseIndex ? "Seek" : "",
                this.Field,
                _equals ? "=" : "",
                _value);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	internal class QueryIn : Query
    {
        private IEnumerable<BsonValue> _values;

        public QueryIn(string field, IEnumerable<BsonValue> values)
            : base(field)
        {
            _values = values ?? Enumerable.Empty<BsonValue>();
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            foreach (var value in _values.Distinct())
            {
                foreach (var node in Query.EQ(this.Field, value).ExecuteIndex(indexer, index))
                {
                    yield return node;
                }
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            foreach(var val in this.Expression.Execute(doc, true))
            {
                foreach (var value in _values.Distinct())
                {
                    var diff = val.CompareTo(value);

                    if (diff == 0) return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}({1} in {2})",
                this.UseFilter ? "Filter" : this.UseIndex ? "Seek" : "",
                this.Field,
                string.Join(",", _values.Select(a => a != null ? a.ToString() : "Null" ).ToArray()));
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	internal class QueryLess : Query
    {
        private BsonValue _value;
        private bool _equals;

        public BsonValue Value { get { return _value; } }
        public bool IsEquals { get { return _equals; } }

        public QueryLess(string field, BsonValue value, bool equals)
            : base(field)
        {
            _value = value;
            _equals = equals;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            foreach (var node in indexer.FindAll(index, Query.Ascending))
            {
                // compares only with are same type
                if (node.Key.Type == _value.Type || (node.Key.IsNumber && _value.IsNumber))
                {
                    var diff = node.Key.CompareTo(_value);

                    if (diff == 1 || (!_equals && diff == 0)) break;

                    if (node.IsHeadTail(index)) yield break;

                    yield return node;
                }
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return this.Expression.Execute(doc, true)
                .Where(x => x.Type == _value.Type || (x.IsNumber && _value.IsNumber))
                .Any(x => x.CompareTo(_value) <= (_equals ? 0 : -1));
        }

        public override string ToString()
        {
            return string.Format("{0}({1} <{2} {3})",
                this.UseFilter ? "Filter" : this.UseIndex ? "Seek" : "",
                this.Field,
                _equals ? "=" : "",
                _value);
        }

    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Not is an Index Scan operation
    /// </summary>
    internal class QueryNot : Query
    {
        private Query _query;
        private int _order;

        public QueryNot(Query query, int order)
            : base("_id")
        {
            _query = query;
            _order = order;
        }

        internal override IEnumerable<IndexNode> Run(CollectionPage col, IndexService indexer)
        {
           // run base query
            var result = _query.Run(col, indexer);

            this.UseIndex = _query.UseIndex;
            this.UseFilter = _query.UseFilter;

            if (_query.UseIndex)
            {
                // if is by index, resolve here
                var all = new QueryAll("_id", _order).Run(col, indexer);

                return all.Except(result, new IndexNodeComparer());
            }
            else
            {
                // if is by document, must return all nodes to be ExecuteDocument after
                return result;
            }
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            throw new NotSupportedException();
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return !_query.FilterDocument(doc);
        }

        public override string ToString()
        {
            return string.Format("!({0})", _query);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Not is an Index Scan operation
	/// </summary>
	internal class QueryNotEquals : Query
    {
        private BsonValue _value;

        public QueryNotEquals(string field, BsonValue value)
            : base(field)
        {
            _value = value;
        }


        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            return indexer
                .FindAll(index, Query.Ascending)
                .Where(x => x.Key.CompareTo(_value) != 0);
        }


        internal override bool FilterDocument(BsonDocument doc)
        {
            return this.Expression.Execute(doc, true)
                .Any(x => x.CompareTo(_value) != 0);
        }

        public override string ToString()
        {
            return string.Format("{0}({1} != {2})",
                this.UseFilter ? "Filter" : this.UseIndex ? "Scan" : "",
                this.Field,
                _value);
        }

    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    internal class QueryOr : Query
    {
        private Query _left;
        private Query _right;

        public QueryOr(Query left, Query right)
            : base(null)
        {
            _left = left;
            _right = right;
        }

        internal override IEnumerable<IndexNode> Run(CollectionPage col, IndexService indexer)
        {
            var left = _left.Run(col, indexer);
            var right = _right.Run(col, indexer);

            // if any query (left/right) is FullScan, this query is full scan too
            this.UseIndex = _left.UseIndex && _right.UseIndex;
            this.UseFilter = _left.UseFilter || _right.UseFilter;

            return left.Union(right, new IndexNodeComparer());
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            throw new NotSupportedException();
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return _left.FilterDocument(doc) || _right.FilterDocument(doc);
        }

        public override string ToString()
        {
            return string.Format("({0} or {1})", _left, _right);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	internal class QueryStartsWith : Query
    {
        private BsonValue _value;

        public QueryStartsWith(string field, BsonValue value)
            : base(field)
        {
            _value = value;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            // find first indexNode
            var node = indexer.Find(index, _value, true, Query.Ascending);
            var str = _value.AsString;

            // navigate using next[0] do next node - if less or equals returns
            while (node != null)
            {
                var valueString = node.Key.AsString;

                // value will not be null because null occurs before string (bsontype sort order)
                if (valueString.StartsWith(str))
                {
                    if (!node.DataBlock.IsEmpty)
                    {
                        yield return node;
                    }
                }
                else
                {
                    break; // if no more starts with, stop scanning
                }

                node = indexer.GetNode(node.Next[0]);
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return this.Expression.Execute(doc, false)
                .Where(x => x.IsString)
                .Any(x => x.AsString.StartsWith(_value));
        }

        public override string ToString()
        {
            return string.Format("{0}({1} startsWith {2})",
                this.UseFilter ? "Filter" : this.UseIndex ? "Seek" : "",
                this.Field,
                _value);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Execute an index scan passing a Func as where
    /// </summary>
    internal class QueryWhere : Query
    {
        private Func<BsonValue, bool> _func;
        private int _order;

        public QueryWhere(string field, Func<BsonValue, bool> func, int order)
            : base(field)
        {
            _func = func;
            _order = order;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            return indexer
                .FindAll(index, _order)
                .Where(i => _func(i.Key));
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return this.Expression.Execute(doc, true)
                .Any(x => _func(x));
        }

        public override string ToString()
        {
            return string.Format("{0}({1}[{2}])",
                this.UseFilter ? "Filter" : this.UseIndex ? "Scan" : "",
                _func.ToString(),
                this.Field);
        }
    }
}


#pragma warning restore
#endregion
