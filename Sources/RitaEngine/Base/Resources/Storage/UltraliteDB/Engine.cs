using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

#pragma warning disable
#region ENGINE ENGINE

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Returns first value from an index (first is min value)
        /// </summary>
        public BsonValue Min(string collection, string field)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            var col = this.GetCollectionPage(collection, false);

            if (col == null) return BsonValue.MinValue;

            // get index (no index, no min)
            var index = col.GetIndex(field);

            if (index == null) return BsonValue.MinValue;

            var head = _indexer.GetNode(index.HeadNode);
            var next = _indexer.GetNode(head.Next[0]);

            if (next.IsHeadTail(index)) return BsonValue.MinValue;

            return next.Key;
            
        }

        /// <summary>
        /// Returns last value from an index (last is max value)
        /// </summary>
        public BsonValue Max(string collection, string field)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            var col = this.GetCollectionPage(collection, false);

            if (col == null) return BsonValue.MaxValue;

            // get index (no index, no max)
            var index = col.GetIndex(field);

            if (index == null) return BsonValue.MaxValue;

            var tail = _indexer.GetNode(index.TailNode);
            var prev = _indexer.GetNode(tail.Prev[0]);

            if (prev.IsHeadTail(index)) return BsonValue.MaxValue;

            return prev.Key;
        
        }

        /// <summary>
        /// Count all nodes from a query execution - do not deserialize documents to count. If query is null, use Collection counter variable
        /// </summary>
        public long Count(string collection, Query query = null)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));

            var col = this.GetCollectionPage(collection, false);

            if (col == null) return 0;

            if (query == null) return col.DocumentCount;

            // run query in this collection
            var nodes = query.Run(col, _indexer);

            if (query.UseFilter)
            {
                // count distinct documents
                return nodes
                    .Select(x => BsonReader.Deserialize(_data.Read(x.DataBlock)).AsDocument)
                    .Where(x => query.FilterDocument(x))
                    .Distinct()
                    .LongCount();
            }
            else
            {
                // count distinct nodes based on DataBlock
                return nodes
                    .Select(x => x.DataBlock)
                    .Distinct()
                    .LongCount();
            }
        }

        /// <summary>
        /// Check if has at least one node in a query execution - do not deserialize documents to check
        /// </summary>
        public bool Exists(string collection, Query query)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (query == null) throw new ArgumentNullException(nameof(query));


            var col = this.GetCollectionPage(collection, false);

            if (col == null) return false;

            // run query in this collection
            var nodes = query.Run(col, _indexer);

            if (query.UseFilter)
            {
                // check if has at least first document
                return nodes
                    .Select(x => BsonReader.Deserialize(_data.Read(x.DataBlock)).AsDocument)
                    .Where(x => query.FilterDocument(x))
                    .Any();
            }
            else
            {
                var first = nodes.FirstOrDefault();

                // check if has at least first node
                return first != null;
            }
        
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Returns all collection inside datafile
        /// </summary>
        public IEnumerable<string> GetCollectionNames()
        {

            var header = _pager.GetPage<HeaderPage>(0);

            return header.CollectionPages.Keys.AsEnumerable();
            
        }

        /// <summary>
        /// Drop collection including all documents, indexes and extended pages
        /// </summary>
        public bool DropCollection(string collection)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));

            return this.Transaction<bool>(collection, false, (col) =>
            {
                if (col == null) return false;

                _log.Write(Logger.COMMAND, "drop collection {0}", collection);

                _collections.Drop(col);

                return true;
            });
        }

        /// <summary>
        /// Rename a collection
        /// </summary>
        public bool RenameCollection(string collection, string newName)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (newName.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(newName));

            return this.Transaction<bool>(collection, false, (col) =>
            {
                if (col == null) return false;

                _log.Write(Logger.COMMAND, "rename collection '{0}' -> '{1}'", collection, newName);

                // check if newName already exists
                if (this.GetCollectionNames().Contains(newName, StringComparer.OrdinalIgnoreCase))
                {
                    throw UltraLiteException.AlreadyExistsCollectionName(newName);
                }

                // change collection name and save
                col.CollectionName = newName;

                // set collection page as dirty
                _pager.SetDirty(col);

                // update header collection reference
                var header = _pager.GetPage<HeaderPage>(0);

                header.CollectionPages.Remove(collection);
                header.CollectionPages.Add(newName, col.PageID);

                _pager.SetDirty(header);

                return true;
            });
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Implement delete command based on _id value. Returns true if deleted
        /// </summary>
        public bool Delete(string collection, BsonValue id)
        {
            return this.Delete(collection, Query.EQ("_id", id)) == 1;
        }

        /// <summary>
        /// Implements delete based on a query result
        /// </summary>
        public int Delete(string collection, Query query)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (query == null) throw new ArgumentNullException(nameof(query));

            return this.Transaction<int>(collection, false, (col) =>
            {
                if (col == null) return 0;

                _log.Write(Logger.COMMAND, "delete documents in '{0}'", collection);

                var nodes = query.Run(col, _indexer);

                _log.Write(Logger.QUERY, "{0} :: {1}", collection, query);

                var count = 0;

                foreach (var node in nodes)
                {
                    // checks if cache are full
                    _trans.CheckPoint();

                    // if use filter need deserialize document
                    if (query.UseFilter)
                    {
                        var buffer = _data.Read(node.DataBlock);
                        var doc = BsonReader.Deserialize(buffer).AsDocument;

                        if (query.FilterDocument(doc) == false) continue;
                    }
                    
                    _log.Write(Logger.COMMAND, "delete document :: _id = {0}", node.Key.RawValue);

                    // get all indexes nodes from this data block
                    var allNodes = _indexer.GetNodeList(node, true).ToArray();

                    // lets remove all indexes that point to this in dataBlock
                    foreach (var linkNode in allNodes)
                    {
                        var index = col.Indexes[linkNode.Slot];

                        _indexer.Delete(index, linkNode.Position);
                    }

                    // remove object data
                    _data.Delete(col, node.DataBlock);

                    count++;
                }

                return count;
            });
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Find for documents in a collection using Query definition
        /// </summary>
        public IEnumerable<BsonDocument> Find(string collection, Query query, int skip = 0, int limit = int.MaxValue)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (query == null) throw new ArgumentNullException(nameof(query));

            _log.Write(Logger.COMMAND, "query documents in '{0}' => {1}", collection, query);

            using (var cursor = new QueryCursor(query, skip, limit))
            {

                // get my collection page
                var col = this.GetCollectionPage(collection, false);

                // no collection, no documents
                if (col == null) yield break;

                // get nodes from query executor to get all IndexNodes
                cursor.Initialize(query.Run(col, _indexer).GetEnumerator());

                _log.Write(Logger.QUERY, "{0} :: {1}", collection, query);

                // fill buffer with documents 
                cursor.Fetch(_trans, _data);
            

                // returing first documents in buffer
                foreach (var doc in cursor.Documents) yield return doc;

                // if still documents to read, continue
                while (cursor.HasMore)
                {
  
                    cursor.Fetch(_trans, _data);
                    

                    // return documents from buffer
                    foreach (var doc in cursor.Documents) yield return doc;
                }
            }
        }

        #region FindOne/FindById

        /// <summary>
        /// Find first or default document based in collection based on Query filter
        /// </summary>
        public BsonDocument FindOne(string collection, Query query)
        {
            return this.Find(collection, query).FirstOrDefault();
        }

        /// <summary>
        /// Find first or default document based in _id field
        /// </summary>
        public BsonDocument FindById(string collection, BsonValue id)
        {
            if (id == null || id.IsNull) throw new ArgumentNullException(nameof(id));

            return this.Find(collection, Query.EQ("_id", id)).FirstOrDefault();
        }


        /// <summary>
        /// Returns all documents inside collection order by _id index.
        /// </summary>
        public IEnumerable<BsonDocument> FindAll(string collection)
        {
            return this.Find(collection, Query.All());
        }

        #endregion
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {

        /// <summary>
        /// Create a new index (or do nothing if already exists) to a collection/field
        /// </summary>
        public bool EnsureIndex(string collection, string field, bool unique = false)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (!CollectionIndex.IndexPattern.IsMatch(field)) throw new ArgumentException("Invalid field format pattern: " + CollectionIndex.IndexPattern.ToString(), "field");
            if (field == "_id") return false; // always exists

            return this.Transaction<bool>(collection, true, (col) =>
            {
                // check if index already exists
                var current = col.GetIndex(field);

                // if already exists, just exit
                if (current != null)
                {
                    // do not test any difference between current index and new defition
                    return false;
                }

                // create index head
                var index = _indexer.CreateIndex(col);

                index.Field = field;
                index.Unique = unique;

                _log.Write(Logger.COMMAND, "create index on '{0}' :: {1} unique: {2}", collection, index.Field, unique);

                // read all objects (read from PK index)
                foreach (var pkNode in new QueryAll("_id", Query.Ascending).Run(col, _indexer))
                {
                    // read binary and deserialize document
                    var buffer = _data.Read(pkNode.DataBlock);
                    var doc = BsonReader.Deserialize(buffer).AsDocument;
                    var expr = new BsonFields(index.Field);

                    // get value from document
                    var keys = expr.Execute(doc, true);

                    // adding index node for each value
                    foreach (var key in keys)
                    {
                        // insert new index node
                        var node = _indexer.AddNode(index, key, pkNode);

                        // link index node to datablock
                        node.DataBlock = pkNode.DataBlock;
                    }

                    // check memory usage
                    _trans.CheckPoint();
                }

                return true;
            });
        }

        /// <summary>
        /// Drop an index from a collection
        /// </summary>
        public bool DropIndex(string collection, string field)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            if (field == "_id") throw UltraLiteException.IndexDropId();

            return this.Transaction<bool>(collection, false, (col) =>
            {
                // no collection, no index
                if (col == null) return false;

                // search for index reference
                var index = col.GetIndex(field);

                // no index, no drop
                if (index == null) return false;

                _log.Write(Logger.COMMAND, "drop index on '{0}' :: '{1}'", collection, field);

                // delete all data pages + indexes pages
                _indexer.DropIndex(index);

                // clear index reference
                index.Clear();

                // mark collection page as dirty
                _pager.SetDirty(col);

                return true;
            });
        }

        /// <summary>
        /// List all indexes inside a collection
        /// </summary>
        public IEnumerable<IndexInfo> GetIndexes(string collection)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));

            var col = this.GetCollectionPage(collection, false);

            if (col == null) yield break;

            foreach (var index in col.GetIndexes(true))
            {
                yield return new IndexInfo(index);
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Get internal information about database. Can filter collections
        /// </summary>
        public BsonDocument Info()
        {
            var header = _pager.GetPage<HeaderPage>(0);
            var collections = new BsonArray();

            foreach(var colName in header.CollectionPages.Keys)
            {
                var col = this.GetCollectionPage(colName, false);

                var colDoc = new BsonDocument
                {
                    { "name", col.CollectionName },
                    { "pageID", (double)col.PageID },
                    { "count", col.DocumentCount },
                    { "sequence", col.Sequence },
                    { "indexes", new BsonArray(
                        col.Indexes.Where(x => !x.IsEmpty).Select(i => new BsonDocument
                        {
                            {  "slot", i.Slot },
                            {  "field", i.Field },
                            {  "unique", i.Unique }
                        }))
                    }
                };

                collections.Add(colDoc);
            }

            return new BsonDocument
            {
                { "userVersion", (int)header.UserVersion },
                { "encrypted", header.Password.Any(x => x > 0) },
                { "changeID", (int)header.ChangeID },
                { "lastPageID", (int)header.LastPageID },
                { "fileSize", BasePage.GetSizeOfPages(header.LastPageID + 1) },
                { "collections", collections }
            };
        }
        
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Implements insert documents in a collection - returns _id value
        /// </summary>
        public BsonValue Insert(string collection, BsonDocument doc, BsonAutoId autoId = BsonAutoId.ObjectId)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            Transaction<int>(collection, true, (col) =>
            {
                this.InsertDocument(col, doc, autoId);
                return 1;
            });

            return doc["_id"];
        }

        /// <summary>
        /// Implements insert documents in a collection - use a buffer to commit transaction in each buffer count
        /// </summary>
        public int Insert(string collection, IEnumerable<BsonDocument> docs, BsonAutoId autoId = BsonAutoId.ObjectId)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (docs == null) throw new ArgumentNullException(nameof(docs));

            return this.Transaction<int>(collection, true, (col) =>
            {
                var count = 0;

                foreach (var doc in docs)
                {
                    this.InsertDocument(col, doc, autoId);

                    _trans.CheckPoint();

                    count++;
                }

                return count;
            });
        }

        /// <summary>
        /// Bulk documents to a collection - use data chunks for most efficient insert
        /// </summary>
        public int InsertBulk(string collection, IEnumerable<BsonDocument> docs, int batchSize = 5000, BsonAutoId autoId = BsonAutoId.ObjectId)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (docs == null) throw new ArgumentNullException(nameof(docs));
            if (batchSize < 100 || batchSize > 100000) throw new ArgumentException("batchSize must be a value between 100 and 100000");

            var count = 0;

            foreach(var batch in docs.Batch(batchSize))
            {
                count += this.Insert(collection, batch, autoId);
            }

            return count;
        }

        /// <summary>
        /// Bulk upsert documents to a collection - use data chunks for most efficient insert
        /// </summary>
        public int UpsertBulk(string collection, IEnumerable<BsonDocument> docs, int batchSize = 5000, BsonAutoId autoId = BsonAutoId.ObjectId)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (docs == null) throw new ArgumentNullException(nameof(docs));
            if (batchSize < 100 || batchSize > 100000) throw new ArgumentException("batchSize must be a value between 100 and 100000");

            var count = 0;

            foreach (var batch in docs.Batch(batchSize))
            {
                count += this.Upsert(collection, batch, autoId);
            }

            return count;
        }

        /// <summary>
        /// Internal implementation of insert a document
        /// </summary>
        private void InsertDocument(CollectionPage col, BsonDocument doc, BsonAutoId autoId)
        {
            // collection Sequence was created after release current datafile version. 
            // In this case, Sequence will be 0 but already has documents. Let's fix this
            // ** this code can be removed when datafile change from 7 (HeaderPage.FILE_VERSION) **
            if (col.Sequence == 0 && col.DocumentCount > 0)
            {
                var max = this.Max(col.CollectionName, "_id");

                // if max value is a number, convert to Sequence last value
                // if not, just set sequence as document count
                col.Sequence = (max.IsInt32 || max.IsInt64 || max.IsDouble || max.IsDecimal) ?
                    Convert.ToInt64(max.RawValue) :
                    Convert.ToInt64(col.DocumentCount);
            }

            // increase collection sequence _id
            col.Sequence++;

            _pager.SetDirty(col);

            // if no _id, add one
            if (!doc.RawValue.TryGetValue("_id", out var id))
            {
                doc["_id"] = id =
                    autoId == BsonAutoId.ObjectId ? new BsonValue(ObjectId.NewObjectId()) :
                    autoId == BsonAutoId.Guid ? new BsonValue(Guid.NewGuid()) :
                    autoId == BsonAutoId.Int32 ? new BsonValue((Int32)col.Sequence) :
                    autoId == BsonAutoId.Int64 ? new BsonValue(col.Sequence) : BsonValue.Null;
            }
            // create bubble in sequence number if _id is bigger than current sequence
            else if(autoId == BsonAutoId.Int32 || autoId == BsonAutoId.Int64)
            {
                var current = id.AsInt64;

                // if current id is bigger than sequence, jump sequence to this number. Other was, do not increse sequnce
                col.Sequence = current >= col.Sequence ? current : col.Sequence - 1;
            }

            // test if _id is a valid type
            if (id.IsNull || id.IsMinValue || id.IsMaxValue)
            {
                throw UltraLiteException.InvalidDataType("_id", id);
            }

            _log.Write(Logger.COMMAND, "insert document on '{0}' :: _id = {1}", col.CollectionName, id.RawValue);

            // serialize object
            var bytes = BsonWriter.Serialize(doc);

            // storage in data pages - returns dataBlock address
            var dataBlock = _data.Insert(col, bytes);

            // store id in a PK index [0 array]
            var pk = _indexer.AddNode(col.PK, id, null);

            // do link between index <-> data block
            pk.DataBlock = dataBlock.Position;

            // for each index, insert new IndexNode
            foreach (var index in col.GetIndexes(false))
            {
                // for each index, get all keys (support now multi-key) - gets distinct values only
                // if index are unique, get single key only
                var expr = new BsonFields(index.Field);
                var keys = expr.Execute(doc, true);

                // do a loop with all keys (multi-key supported)
                foreach(var key in keys)
                {
                    // insert node
                    var node = _indexer.AddNode(index, key, pk);

                    // link my index node to data block address
                    node.DataBlock = dataBlock.Position;
                }
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Reduce disk size re-arranging unused spaces. Can change password. If temporary disk was not provided, use MemoryStream temp disk
        /// </summary>
        public long Shrink(string password = null, IDiskService tempDisk = null)
        {
            var originalSize = _disk.FileLength;

            // if temp disk are not passed, use memory stream disk
            using (var temp = tempDisk ?? new StreamDiskService(new MemoryStream()))
            using (var engine = new UltraLiteEngine(temp, password))
            {
                // read all collection
                foreach (var collectionName in this.GetCollectionNames())
                {
                    // first create all user indexes (exclude _id index)
                    foreach (var index in this.GetIndexes(collectionName).Where(x => x.Field != "_id"))
                    {
                        engine.EnsureIndex(collectionName, index.Field, index.Unique);
                    }
                    
                    // now copy documents 
                    var docs = this.Find(collectionName, Query.All());

                    engine.InsertBulk(collectionName, docs);

                    // fix collection sequence number
                    var seq = _collections.Get(collectionName).Sequence;

                    engine.Transaction(collectionName, true, (col) =>
                    {
                        col.Sequence = seq;
                        engine._pager.SetDirty(col);
                        return true;
                    });

                }

                // copy user version
                engine.UserVersion = this.UserVersion;

                // set current disk size to exact new disk usage
                _disk.SetLength(temp.FileLength);

                // read new header page to start copy
                var header = BasePage.ReadPage(temp.ReadPage(0)) as HeaderPage;

                // copy (as is) all pages from temp disk to original disk
                for (uint i = 0; i <= header.LastPageID; i++)
                {
                    // skip lock page
                    if (i == 1) continue;

                    var page = temp.ReadPage(i);

                    _disk.WritePage(i, page);
                }

                // create/destroy crypto class
                if (_crypto != null) _crypto.Dispose();

                _crypto = password == null ? null : new AesEncryption(password, header.Salt);

                // initialize all services again (crypto can be changed)
                this.InitializeServices();
                
                // return how many bytes are reduced
                return originalSize - temp.FileLength;
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Implement update command to a document inside a collection. Returns true if document was updated
        /// </summary>
        public bool Update(string collection, BsonDocument doc)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            return this.Transaction<bool>(collection, false, (col) =>
            {
                // no collection, no updates
                if (col == null) return false;

                var updated = false;

                if (this.UpdateDocument(col, doc))
                {
                    updated = true;
                }

                return updated;
            });
        }

        /// <summary>
        /// Implement update command to a document inside a collection. Return number of documents updated
        /// </summary>
        public int Update(string collection, IEnumerable<BsonDocument> docs)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (docs == null) throw new ArgumentNullException(nameof(docs));

            return this.Transaction<int>(collection, false, (col) =>
            {
                // no collection, no updates
                if (col == null) return 0;

                var count = 0;

                foreach (var doc in docs)
                {
                    if (this.UpdateDocument(col, doc))
                    {
                        _trans.CheckPoint();
                        count++;
                    }
                }

                return count;
            });
        }

        /// <summary>
        /// Implement internal update document
        /// </summary>
        private bool UpdateDocument(CollectionPage col, BsonDocument doc)
        {
            // normalize id before find
            var id = doc["_id"];

            // validate id for null, min/max values
            if (id.IsNull || id.IsMinValue || id.IsMaxValue)
            {
                throw UltraLiteException.InvalidDataType("_id", id);
            }

            _log.Write(Logger.COMMAND, "update document on '{0}' :: _id = {1}", col.CollectionName, id.RawValue);

            // find indexNode from pk index
            var pkNode = _indexer.Find(col.PK, id, false, Query.Ascending);

            // if not found document, no updates
            if (pkNode == null) return false;

            // serialize document in bytes
            var bytes = BsonWriter.Serialize(doc);

            // update data storage
            var dataBlock = _data.Update(col, pkNode.DataBlock, bytes);

            // get all non-pk index nodes from this data block
            var allNodes = _indexer.GetNodeList(pkNode, false).ToArray();

            // delete/insert indexes - do not touch on PK
            foreach (var index in col.GetIndexes(false))
            {
                var expr = new BsonFields(index.Field);

                // getting all keys do check
                var keys = expr.Execute(doc).ToArray();

                // get a list of to delete nodes (using ToArray to resolve now)
                var toDelete = allNodes
                    .Where(x => x.Slot == index.Slot && !keys.Any(k => k == x.Key))
                    .ToArray();

                // get a list of to insert nodes (using ToArray to resolve now)
                var toInsert = keys
                    .Where(x => !allNodes.Any(k => k.Slot == index.Slot && k.Key == x))
                    .ToArray();

                // delete changed index nodes
                foreach (var node in toDelete)
                {
                    _indexer.Delete(index, node.Position);
                }

                // insert new nodes
                foreach (var key in toInsert)
                {
                    // and add a new one
                    var node = _indexer.AddNode(index, key, pkNode);

                    // link my node to data block
                    node.DataBlock = dataBlock.Position;
                }
            }

            return true;
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Implement upsert command to documents in a collection. Calls update on all documents,
        /// then any documents not updated are then attempted to insert.
        /// This will have the side effect of throwing if duplicate items are attempted to be inserted. Returns true if document is inserted
        /// </summary>
        public bool Upsert(string collection, BsonDocument doc, BsonAutoId autoId = BsonAutoId.ObjectId)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            return this.Transaction<bool>(collection, true, (col) =>
            {
                var inserted = false;

                // first try update document (if exists _id)
                // if not found, insert
                if (doc["_id"] == BsonValue.Null || this.UpdateDocument(col, doc) == false)
                {
                    this.InsertDocument(col, doc, autoId);
                    inserted = true;
                }

                // returns if document was inserted
                return inserted;
            });
        }

        /// <summary>
        /// Implement upsert command to documents in a collection. Calls update on all documents,
        /// then any documents not updated are then attempted to insert.
        /// This will have the side effect of throwing if duplicate items are attempted to be inserted.
        /// </summary>
        public int Upsert(string collection, IEnumerable<BsonDocument> docs, BsonAutoId autoId = BsonAutoId.ObjectId)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (docs == null) throw new ArgumentNullException(nameof(docs));

            return this.Transaction<int>(collection, true, (col) =>
            {
                var count = 0;

                foreach (var doc in docs)
                {
                    // first try update document (if exists _id)
                    // if not found, insert
                    if (doc["_id"] == BsonValue.Null || this.UpdateDocument(col, doc) == false)
                    {
                        this.InsertDocument(col, doc, autoId);
                        count++;
                    }

                    _trans.CheckPoint();
                }

                // returns how many document was inserted
                return count;
            });
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Get/Set User version internal database
        /// </summary>
        public ushort UserVersion
        {
            get
            {

                var header = _pager.GetPage<HeaderPage>(0);

                return header.UserVersion;
            
            }
            set
            {
                this.Transaction<bool>(null, false, (col) =>
                {
                    var header = _pager.GetPage<HeaderPage>(0);

                    header.UserVersion = value;

                    _pager.SetDirty(header);

                    return true;
                });
            }
        }
    }
}

#endregion

#region  ENGINE

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// A public class that take care of all engine data structure access - it´s basic implementation of a NoSql database
    /// Its isolated from complete solution - works on low level only (no linq, no poco... just Bson objects)
    /// </summary>
    public partial class UltraLiteEngine : IDisposable
    {
        #region Services instances

        private Logger _log;

        private IDiskService _disk;

        private CacheService _cache;

        private PageService _pager;

        private TransactionService _trans;

        private IndexService _indexer;

        private DataService _data;

        private CollectionService _collections;

        private AesEncryption _crypto;

        private int _cacheSize;

        private TimeSpan _timeout;

        /// <summary>
        /// Get log instance for debug operations
        /// </summary>
        public Logger Log { get { return _log; } }

        /// <summary>
        /// Get memory cache size limit. Works only with journal enabled (number in pages). If journal is disabled, pages in cache can exceed this limit. Default is 5000 pages
        /// </summary>
        public int CacheSize { get { return _cacheSize; } }

        /// <summary>
        /// Get how many pages are on cache
        /// </summary>
        public int CacheUsed { get { return _cache.CleanUsed; } }

        /// <summary>
        /// Gets time waiting write lock operation before throw UltraLiteException timeout
        /// </summary>
        public TimeSpan Timeout { get { return _timeout; } }


        #endregion

        #region Ctor

        /// <summary>
        /// Initialize UltraLiteEngine using default FileDiskService
        /// </summary>
        public UltraLiteEngine(string filename, bool journal = true)
            : this(new FileDiskService(filename, journal))
        {
        }

        /// <summary>
        /// Initialize UltraLiteEngine with password encryption
        /// </summary>
        public UltraLiteEngine(string filename, string password, bool journal = true)
            : this(new FileDiskService(filename, new FileOptions { Journal = journal }), password)
        {
        }

        /// <summary>
        /// Initialize UltraLiteEngine using StreamDiskService
        /// </summary>
        public UltraLiteEngine(Stream stream, string password = null)
            : this(new StreamDiskService(stream), password)
        {
        }

        /// <summary>
        /// Initialize UltraLiteEngine using custom disk service implementation and full engine options
        /// </summary>
        public UltraLiteEngine(IDiskService disk, string password = null, TimeSpan? timeout = null, int cacheSize = 5000, Logger log = null, bool utcDate = false)
        {
            if (disk == null) throw new ArgumentNullException(nameof(disk));

            _timeout = timeout ?? TimeSpan.FromMinutes(1);
            _cacheSize = cacheSize;
            _disk = disk;
            _log = log ?? new Logger();

            try
            {
                // initialize datafile (create) and set log instance
                _disk.Initialize(_log, password);

                var buffer = _disk.ReadPage(0);

                // create header instance from array bytes
                var header = BasePage.ReadPage(buffer) as HeaderPage;

                // hash password with sha1 or keep as empty byte[20]
                var sha1 = password == null ? new byte[20] : AesEncryption.HashSHA1(password);

                // compare header password with user password even if not passed password (datafile can have password)
                if (sha1.BinaryCompareTo(header.Password) != 0)
                {
                    throw UltraLiteException.DatabaseWrongPassword();
                }

                // initialize AES encryptor
                if (password != null)
                {
                    _crypto = new AesEncryption(password, header.Salt);
                }

                // initialize all services
                this.InitializeServices();

                // if header are marked with recovery, do it now
                if (header.Recovery)
                {
                    _trans.Recovery();
                }
            }
            catch (Exception)
            {
                // explicit dispose
                this.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Create instances for all engine services
        /// </summary>
        private void InitializeServices()
        {
            _cache = new CacheService(_disk, _log);
            _pager = new PageService(_disk, _crypto, _cache, _log);
            _indexer = new IndexService(_pager, _log);
            _data = new DataService(_pager, _log);
            _trans = new TransactionService(_disk, _crypto, _pager, _cache, _cacheSize, _log);
            _collections = new CollectionService(_pager, _indexer, _data, _trans, _log);
        }

        #endregion

        /// <summary>
        /// Get the collection page only when needed. Gets from pager always to grantee that wil be the last (in case of clear cache will get a new one - pageID never changes)
        /// </summary>
        private CollectionPage GetCollectionPage(string name, bool addIfNotExits)
        {
            if (name == null) return null;

            // search my page on collection service
            var col = _collections.Get(name);

            if (col == null && addIfNotExits)
            {
                _log.Write(Logger.COMMAND, "create new collection '{0}'", name);

                col = _collections.Add(name);
            }

            return col;
        }

        /// <summary>
        /// Encapsulate all operations in a single write transaction
        /// </summary>
        private T Transaction<T>(string collection, bool addIfNotExists, Func<CollectionPage, T> action)
        {

            try
            {
                var col = this.GetCollectionPage(collection, addIfNotExists);

                var result = action(col);

                _trans.PersistDirtyPages();

                _trans.CheckPoint();
                
                return result;
            }
            catch (Exception ex)
            {
                _log.Write(Logger.ERROR, ex.Message);

                // if an error occurs during an operation, rollback must be called to avoid datafile inconsistent
                _cache.DiscardDirtyPages();

                throw;
            }
            
        }

        public void Dispose()
        {
            // dispose datafile and journal file
            _disk.Dispose();

            // dispose crypto
            if (_crypto != null) _crypto.Dispose();
        }

        /// <summary>
        /// Initialize new datafile with header page + lock reserved area zone
        /// </summary>
        public static void CreateDatabase(Stream stream, string password = null, long initialSize = 0)
        {
            // calculate how many empty pages will be added on disk
            var emptyPages = initialSize == 0 ? 0 : (initialSize - (2 * BasePage.PAGE_SIZE)) / BasePage.PAGE_SIZE;

            // if too small size (less than 2 pages), assume no initial size
            if (emptyPages < 0) emptyPages = 0;

            // create a new header page in bytes (keep second page empty)
            var header = new HeaderPage
            {
                LastPageID = initialSize == 0 ? 1 : (uint)emptyPages + 1,
                FreeEmptyPageID = initialSize == 0 ? uint.MaxValue : 2
            };

            if (password != null)
            {
                header.Password = AesEncryption.HashSHA1(password);
                header.Salt = AesEncryption.Salt();
            }

            // point to begin file
            stream.Seek(0, SeekOrigin.Begin);

            // get header page in bytes
            var buffer = header.WritePage();

            stream.Write(buffer, 0, BasePage.PAGE_SIZE);

            // write second page as an empty AREA (it's not a page) just to use as lock control
            stream.Write(new byte[BasePage.PAGE_SIZE], 0, BasePage.PAGE_SIZE);

            // create crypto class if has password
            var crypto = password != null ? new AesEncryption(password, header.Salt) : null;

            // if initial size is defined, lets create empty pages in a linked list
            if (emptyPages > 0)
            {
                stream.SetLength(initialSize);

                var pageID = 1u;

                while(++pageID < (emptyPages + 2))
                {
                    var empty = new EmptyPage(pageID)
                    {
                        PrevPageID = pageID == 2 ? 0 : pageID - 1,
                        NextPageID = pageID == emptyPages + 1 ? uint.MaxValue : pageID + 1
                    };

                    var bytes = empty.WritePage();

                    if (password != null)
                    {
                        bytes = crypto.Encrypt(bytes);
                    }

                    stream.Write(bytes, 0, BasePage.PAGE_SIZE);
                }
            }

            if (crypto != null) crypto.Dispose();
        }
    }
}

#endregion
#pragma warning restore