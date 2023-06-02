using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#pragma warning disable

#region DATABASE COLLECTIONS

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteCollection<T>
    {
        #region Count

        /// <summary>
        /// Get document count using property on collection.
        /// </summary>
        public int Count()
        {
            // do not use indexes - collections has DocumentCount property
            return (int)_engine.Value.Count(_name, null);
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public int Count(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return (int)_engine.Value.Count(_name, query);
        }


        #endregion

        #region LongCount

        /// <summary>
        /// Get document count using property on collection.
        /// </summary>
        public long LongCount()
        {
            // do not use indexes - collections has DocumentCount property
            return _engine.Value.Count(_name, null);
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public long LongCount(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return _engine.Value.Count(_name, query);
        }


        #endregion

        #region Exists

        /// <summary>
        /// Returns true if query returns any document. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public bool Exists(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return _engine.Value.Exists(_name, query);
        }


        #endregion

        #region Min/Max

        /// <summary>
        /// Returns the first/min value from a index field
        /// </summary>
        public BsonValue Min(string field)
        {
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException(nameof(field));

            return _engine.Value.Min(_name, field);
        }


        /// <summary>
        /// Returns the last/max value from a index field
        /// </summary>
        public BsonValue Max(string field)
        {
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException(nameof(field));

            return _engine.Value.Max(_name, field);
        }



        #endregion
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteCollection<T>
    {
        /// <summary>
        /// Remove all document based on a Query object. Returns removed document counts
        /// </summary>
        public int Delete(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return _engine.Value.Delete(_name, query);
        }

        /// <summary>
        /// Remove an document in collection using Document Id - returns false if not found document
        /// </summary>
        public bool Delete(BsonValue id)
        {
            if (id == null || id.IsNull) throw new ArgumentNullException(nameof(id));

            return this.Delete(Query.EQ("_id", id)) > 0;
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteCollection<T>
    {
        #region Find

        /// <summary>
        /// Find documents inside a collection using Query object.
        /// </summary>
        public IEnumerable<T> Find(Query query, int skip = 0, int limit = int.MaxValue)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var docs = _engine.Value.Find(_name, query, skip, limit);

            foreach(var doc in docs)
            {
                // get object from BsonDocument
                var obj = _mapper.ToObject<T>(doc);

                yield return obj;
            }
        }


        #endregion

        #region FindById + One + All

        /// <summary>
        /// Find a document using Document Id. Returns null if not found.
        /// </summary>
        public T FindById(BsonValue id)
        {
            if (id == null || id.IsNull) throw new ArgumentNullException(nameof(id));

            return this.Find(Query.EQ("_id", id)).SingleOrDefault();
        }

        /// <summary>
        /// Find the first document using Query object. Returns null if not found. Must have index on query expression.
        /// </summary>
        public T FindOne(Query query)
        {
            return this.Find(query).FirstOrDefault();
        }


        /// <summary>
        /// Returns all documents inside collection order by _id index.
        /// </summary>
        public IEnumerable<T> FindAll()
        {
            return this.Find(Query.All());
        }

        #endregion
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteCollection<T>
    {
        /// <summary>
        /// Create a new permanent index in all documents inside this collections if index not exists already. Returns true if index was created or false if already exits
        /// </summary>
        /// <param name="field">Document field name (case sensitive)</param>
        /// <param name="unique">If is a unique index</param>
        public bool EnsureIndex(string field, bool unique = false)
        // {
            // if (string.IsNullOrEmpty(field)) throw new ArgumentNullException(nameof(field));

            // return _engine.Value.EnsureIndex(_name, field, unique);


            => string.IsNullOrEmpty(field) 
                ? throw new ArgumentNullException(nameof(field)) 
                :  _engine.Value.EnsureIndex(_name, field, unique);
        // }


        /// <summary>
        /// Returns all indexes information
        /// </summary>
        public IEnumerable<IndexInfo> GetIndexes()
        {
            return _engine.Value.GetIndexes(_name);
        }

        /// <summary>
        /// Drop index and release slot for another index
        /// </summary>
        public bool DropIndex(string field)
        {
            return _engine.Value.DropIndex(_name, field);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteCollection<T>
    {
        /// <summary>
        /// Insert a new entity to this collection. Document Id must be a new value in collection - Returns document Id
        /// </summary>
        public BsonValue Insert(T document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            var doc = _mapper.ToDocument(document);
            var removed = this.RemoveDocId(doc);

            var id = _engine.Value.Insert(_name, doc, _autoId);

            // checks if must update _id value in entity
            if (removed && _id != null)
            {
                _id.Setter(document, id.RawValue);
            }

            return id;
        }

        /// <summary>
        /// Insert a new document to this collection using passed id value.
        /// </summary>
        public void Insert(BsonValue id, T document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (id == null || id.IsNull) throw new ArgumentNullException(nameof(id));

            var doc = _mapper.ToDocument(document);

            doc["_id"] = id;

            _engine.Value.Insert(_name, doc);
        }

        /// <summary>
        /// Insert an array of new documents to this collection. Document Id must be a new value in collection. Can be set buffer size to commit at each N documents
        /// </summary>
        public int Insert(IEnumerable<T> docs)
        {
            if (docs == null) throw new ArgumentNullException(nameof(docs));

            return _engine.Value.Insert(_name, this.GetBsonDocs(docs), _autoId);
        }

        /// <summary>
        /// Implements bulk insert documents in a collection. Usefull when need lots of documents.
        /// </summary>
        public int InsertBulk(IEnumerable<T> docs, int batchSize = 5000)
        {
            if (docs == null) throw new ArgumentNullException(nameof(docs));

            return _engine.Value.InsertBulk(_name, this.GetBsonDocs(docs), batchSize, _autoId);
        }

        /// <summary>
        /// Implements bulk upsert of documents in a collection. Usefull when need lots of documents.
        /// </summary>
        public int UpsertBulk(IEnumerable<T> docs, int batchSize = 5000)
        {
            if (docs == null) throw new ArgumentNullException(nameof(docs));

            return _engine.Value.UpsertBulk(_name, this.GetBsonDocs(docs), batchSize, _autoId);
        }

        /// <summary>
        /// Convert each T document in a BsonDocument, setting autoId for each one
        /// </summary>
        private IEnumerable<BsonDocument> GetBsonDocs(IEnumerable<T> documents)
        {
            foreach (var document in documents)
            {
                var doc = _mapper.ToDocument(document);
                var removed = this.RemoveDocId(doc);

                yield return doc;

                if (removed && _id != null)
                {
                    _id.Setter(document, doc["_id"].RawValue);
                }
            }
        }

        /// <summary>
        /// Remove document _id if contains a "empty" value (checks for autoId bson type)
        /// </summary>
        private bool RemoveDocId(BsonDocument doc)
        {
            if (_id != null && doc.TryGetValue("_id", out var id)) 
            {
                // check if exists _autoId and current id is "empty"
                if ((_autoId == BsonAutoId.Int32 && (id.IsInt32 && id.AsInt32 == 0)) ||
                    (_autoId == BsonAutoId.ObjectId && (id.IsNull || (id.IsObjectId && id.AsObjectId == ObjectId.Empty))) ||
                    (_autoId == BsonAutoId.Guid && id.IsGuid && id.AsGuid == Guid.Empty) ||
                    (_autoId == BsonAutoId.Int64 && id.IsInt64 && id.AsInt64 == 0))
                {
                    // in this cases, remove _id and set new value after
                    doc.Remove("_id");
                    return true;
                }
            }

            return false;   
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public sealed partial class UltraLiteCollection<T>
    {
        private string _name;
        private LazyLoad<UltraLiteEngine> _engine;
        private BsonMapper _mapper;
        private readonly EntityMapper _entity;
        private Logger _log;
        private MemberMapper _id;
        private BsonAutoId _autoId;

        /// <summary>
        /// Get collection name
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Getting entity mapper from current collection. Returns null if collection are BsonDocument type
        /// </summary>
        public EntityMapper EntityMapper => _entity;

        public UltraLiteCollection(string name, BsonAutoId autoId, LazyLoad<UltraLiteEngine> engine, BsonMapper mapper, Logger log)
        {
            _name = name ?? mapper.ResolveCollectionName(typeof(T));
            _engine = engine;
            _mapper = mapper;
            _log = log;

            // if strong typed collection, get _id member mapped (if exists)
            if (typeof(T) == typeof(BsonDocument))
            {
                _entity = null;
                _id = null;
                _autoId = autoId;
            }
            else
            {
                _entity = mapper.GetEntityMapper(typeof(T));
                _id = _entity.Id;

                if (_id != null && _id.AutoId)
                {
                    _autoId =
                        _id.DataType == typeof(Int32) || _id.DataType == typeof(Int32?) ? BsonAutoId.Int32 :
                        _id.DataType == typeof(Int64) || _id.DataType == typeof(Int64?) ? BsonAutoId.Int64 :
                        _id.DataType == typeof(Guid) || _id.DataType == typeof(Guid?) ? BsonAutoId.Guid :
                        BsonAutoId.ObjectId;
                }
                else
                {
                    _autoId = BsonAutoId.ObjectId;
                }
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteCollection<T>
    {
        /// <summary>
        /// Update a document in this collection. Returns false if not found document in collection
        /// </summary>
        public bool Update(T document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            // get BsonDocument from object
            var doc = _mapper.ToDocument(document);

            return _engine.Value.Update(_name, new BsonDocument[] { doc }) > 0;
        }

        /// <summary>
        /// Update a document in this collection. Returns false if not found document in collection
        /// </summary>
        public bool Update(BsonValue id, T document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (id == null || id.IsNull) throw new ArgumentNullException(nameof(id));

            // get BsonDocument from object
            var doc = _mapper.ToDocument(document);

            // set document _id using id parameter
            doc["_id"] = id;

            return _engine.Value.Update(_name, new BsonDocument[] { doc }) > 0;
        }

        /// <summary>
        /// Update all documents
        /// </summary>
        public int Update(IEnumerable<T> documents)
        {
            if (documents == null) throw new ArgumentNullException(nameof(documents));

            return _engine.Value.Update(_name, documents.Select(x => _mapper.ToDocument(x)));
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteCollection<T>
    {
        /// <summary>
        /// Insert or Update a document in this collection.
        /// </summary>
        public bool Upsert(T document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            // get BsonDocument from object
            var doc = _mapper.ToDocument(document);

            return _engine.Value.Upsert(_name, doc);
        }

        /// <summary>
        /// Insert or Update all documents
        /// </summary>
        public int Upsert(IEnumerable<T> documents)
        {
            if (documents == null) throw new ArgumentNullException(nameof(documents));

            return _engine.Value.Upsert(_name, this.GetBsonDocs(documents), _autoId);
        }

        /// <summary>
        /// Insert or Update a document in this collection.
        /// </summary>
        public bool Upsert(BsonValue id, T document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (id == null || id.IsNull) throw new ArgumentNullException(nameof(id));

            // get BsonDocument from object
            var doc = _mapper.ToDocument(document);

            // set document _id using id parameter
            doc["_id"] = id;

            return _engine.Value.Upsert(_name, doc);
        }
    }
}

#endregion

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// The LiteDB database. Used for create a LiteDB instance and use all storage resources. It's the database connection
    /// </summary>
    public partial class UltraLiteDatabase : IDisposable
    {
        #region Properties

        private LazyLoad<UltraLiteEngine> _engine = null;
        private BsonMapper _mapper = BsonMapper.Global;
        private Logger _log = null;
        private ConnectionString _connectionString = null;

        /// <summary>
        /// Get logger class instance
        /// </summary>
        public Logger Log { get { return _log; } }

        /// <summary>
        /// Get current instance of BsonMapper used in this database instance (can be BsonMapper.Global)
        /// </summary>
        public BsonMapper Mapper { get { return _mapper; } }

        /// <summary>
        /// Get current database engine instance. Engine is lower data layer that works with BsonDocuments only (no mapper, no LINQ)
        /// </summary>
        public UltraLiteEngine Engine { get { return _engine.Value; } }

        #endregion

        #region Ctor

        /// <summary>
        /// Starts LiteDB database using a connection string for file system database
        /// </summary>
        public UltraLiteDatabase(string connectionString, BsonMapper mapper = null, Logger log = null)
            : this(new ConnectionString(connectionString), mapper, log)
        {
        }

        /// <summary>
        /// Starts LiteDB database using a connection string for file system database
        /// </summary>
        public UltraLiteDatabase(ConnectionString connectionString, BsonMapper mapper = null, Logger log = null)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
            _log = log ?? new Logger();
            _log.Level = log?.Level ?? _connectionString.Log;

            _mapper = mapper ?? BsonMapper.Global;

            var options = new FileOptions
            {
                Async = _connectionString.Async,
                Flush = _connectionString.Flush,
                InitialSize = _connectionString.InitialSize,
                LimitSize = _connectionString.LimitSize,
                Journal = _connectionString.Journal,
            };

            _engine = new LazyLoad<UltraLiteEngine>(() => new UltraLiteEngine(new FileDiskService(_connectionString.Filename, options), _connectionString.Password, _connectionString.Timeout, _connectionString.CacheSize, _log, _connectionString.UtcDate));
        }

        /// <summary>
        /// Starts LiteDB database using a Stream disk
        /// </summary>
        public UltraLiteDatabase(Stream stream, BsonMapper mapper = null, string password = null, bool disposeStream = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _mapper = mapper ?? BsonMapper.Global;
            _log = new Logger();

            _engine = new LazyLoad<UltraLiteEngine>(() => new UltraLiteEngine(new StreamDiskService(stream, disposeStream), password: password, log: _log));
        }

        /// <summary>
        /// Starts LiteDB database using a custom IDiskService with all parameters available
        /// </summary>
        /// <param name="diskService">Custom implementation of persist data layer</param>
        /// <param name="mapper">Instance of BsonMapper that map poco classes to document</param>
        /// <param name="password">Password to encrypt you datafile</param>
        /// <param name="timeout">Locker timeout for concurrent access</param>
        /// <param name="cacheSize">Max memory pages used before flush data in Journal file (when available)</param>
        /// <param name="log">Custom log implementation</param>
        public UltraLiteDatabase(IDiskService diskService, BsonMapper mapper = null, string password = null, TimeSpan? timeout = null, int cacheSize = 5000, Logger log = null)
        {
            if (diskService == null) throw new ArgumentNullException(nameof(diskService));

            _mapper = mapper ?? BsonMapper.Global;
            _log = log ?? new Logger();

            _engine = new LazyLoad<UltraLiteEngine>(() => new UltraLiteEngine(diskService, password: password, timeout: timeout, cacheSize: cacheSize, log: _log ));
        }

        #endregion

        #region Collections

        public UltraLiteCollection<T> GetCollection<T>(string name)
        {
            return new UltraLiteCollection<T>(name, BsonAutoId.ObjectId, _engine, _mapper, _log);
        }

        /// <summary>
        /// Get a collection using a name based on typeof(T).Name (BsonMapper.ResolveCollectionName function)
        /// </summary>
        public UltraLiteCollection<T> GetCollection<T>()
        {
            return this.GetCollection<T>(null);
        }

        /// <summary>
        /// Get a collection using a generic BsonDocument. If collection does not exits, create a new one.
        /// </summary>
        /// <param name="name">Collection name (case insensitive)</param>
        /// <param name="autoId">Define autoId data type (when document contains no _id field)</param>
        public UltraLiteCollection<BsonDocument> GetCollection(string name, BsonAutoId autoId = BsonAutoId.ObjectId)
        {
            if (name.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(name));

            return new UltraLiteCollection<BsonDocument>(name, autoId, _engine, _mapper, _log);
        }

        #endregion


        #region Shortcut

        /// <summary>
        /// Get all collections name inside this database.
        /// </summary>
        public IEnumerable<string> GetCollectionNames()
        {
            return _engine.Value.GetCollectionNames();
        }

        /// <summary>
        /// Checks if a collection exists on database. Collection name is case insensitive
        /// </summary>
        public bool CollectionExists(string name)
        {
            if (name.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(name));

            return _engine.Value.GetCollectionNames().Contains(name, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Drop a collection and all data + indexes
        /// </summary>
        public bool DropCollection(string name)
        {
            if (name.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(name));

            return _engine.Value.DropCollection(name);
        }

        /// <summary>
        /// Rename a collection. Returns false if oldName does not exists or newName already exists
        /// </summary>
        public bool RenameCollection(string oldName, string newName)
        {
            if (oldName.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(oldName));
            if (newName.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(newName));

            return _engine.Value.RenameCollection(oldName, newName);
        }

        #endregion

        #region Shrink

        /// <summary>
        /// Reduce disk size re-arranging unused spaces.
        /// </summary>
        public long Shrink()
        {
            return this.Shrink(_connectionString?.Password);
        }

        /// <summary>
        /// Reduce disk size re-arranging unused space. Can change password. If a temporary disk was not provided, use MemoryStream temp disk
        /// </summary>
        public long Shrink(string password)
        {
            // if has connection string, use same path
            if (_connectionString != null)
            {
                // get temp file ("-temp" suffix)
                var tempFile = FileHelper.GetTempFile(_connectionString.Filename);
                var reduced = 0L;

                try
                {
                    // get temp disk based on temp file
                    var tempDisk = new FileDiskService(tempFile, new FileOptions { Journal = false });

                    reduced = _engine.Value.Shrink(password, tempDisk);
                }
                finally
                {
                    // delete temp file
                    File.Delete(tempFile);
                }

                return reduced;
            }
            else
            {
                return _engine.Value.Shrink(password);
            }
        }

        #endregion

        public void Dispose()
        {
            if (_engine.IsValueCreated) _engine.Value.Dispose();
        }
    }
}

#pragma warning restore