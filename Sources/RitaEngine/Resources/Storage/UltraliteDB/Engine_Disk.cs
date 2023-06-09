using System;
using System.Collections.Generic;
using System.IO;

#pragma warning disable

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Implement NTFS File disk
    /// </summary>
    public class FileDiskService : IDiskService
    {
        /// <summary>
        /// Position, on page, about page type
        /// </summary>
        private const int PAGE_TYPE_POSITION = 4;

        /// <summary>
        /// Map lock positions
        /// </summary>
        internal const int LOCK_INITIAL_POSITION = BasePage.PAGE_SIZE; // use second page
        internal const int LOCK_READ_LENGTH = 1;
        internal const int LOCK_WRITE_LENGTH = 3000;

        private FileStream _stream;
        private string _filename;

        private Logger _log; // will be initialize in "Initialize()"
        private FileOptions _options;

        private Random _lockReadRand = new Random();

        #region Initialize/Dispose disk

        public FileDiskService(string filename, bool journal = true)
            : this(filename, new FileOptions { Journal = journal })
        {
        }

        public FileDiskService(string filename, FileOptions options)
        {
            // simple validations
            if (filename.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filename));
            if (options.InitialSize > options.LimitSize) throw new ArgumentException("limit size less than initial size");

            // setting class variables
            _filename = filename;
            _options = options;
        }

        public void Initialize(Logger log, string password)
        {
            // get log instance to disk
            _log = log;

            _log.Write(Logger.DISK, "open datafile '{0}'", Path.GetFileName(_filename));

            // open/create file using read only/exclusive options
            _stream = this.CreateFileStream(_filename,
                System.IO.FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.None);

            // if file is new, initialize
            if (_stream.Length == 0)
            {
                _log.Write(Logger.DISK, "initialize new datafile");

                // create datafile
                UltraLiteEngine.CreateDatabase(_stream, password, _options.InitialSize);
            }
        }

        public virtual void Dispose()
        {
            if (_stream != null)
            {
                _log.Write(Logger.DISK, "close datafile '{0}'", Path.GetFileName(_filename));
                _stream.Dispose();
                _stream = null;
            }
        }

        #endregion

        #region Read/Write

        /// <summary>
        /// Read page bytes from disk
        /// </summary>
        public virtual byte[] ReadPage(uint pageID)
        {
            var buffer = new byte[BasePage.PAGE_SIZE];
            var position = BasePage.GetSizeOfPages(pageID);

            lock (_stream)
            {
                // position cursor
                if (_stream.Position != position)
                {
                    _stream.Seek(position, SeekOrigin.Begin);
                }

                // read bytes from data file
                _stream.Read(buffer, 0, BasePage.PAGE_SIZE);
            }

            _log.Write(Logger.DISK, "read page #{0:0000} :: {1}", pageID, (PageType)buffer[PAGE_TYPE_POSITION]);

            return buffer;
        }

        /// <summary>
        /// Persist single page bytes to disk
        /// </summary>
        public virtual void WritePage(uint pageID, byte[] buffer)
        {
            var position = BasePage.GetSizeOfPages(pageID);

            _log.Write(Logger.DISK, "write page #{0:0000} :: {1}", pageID, (PageType)buffer[PAGE_TYPE_POSITION]);

            // position cursor
            if (_stream.Position != position)
            {
                _stream.Seek(position, SeekOrigin.Begin);
            }

            _stream.Write(buffer, 0, BasePage.PAGE_SIZE);
        }

        /// <summary>
        /// Set datafile length
        /// </summary>
        public void SetLength(long fileSize)
        {
            // checks if new fileSize will exceed limit size
            if (fileSize > _options.LimitSize) throw UltraLiteException.FileSizeExceeded(_options.LimitSize);

            // fileSize parameter tell me final size of data file - helpful to extend first datafile
            _stream.SetLength(fileSize);
        }

        /// <summary>
        /// Returns file length
        /// </summary>
        public long FileLength { get { return _stream.Length; } }

        #endregion

        #region Journal file

        /// <summary>
        /// Indicate if journal are enabled or not based on file options
        /// </summary>
        public bool IsJournalEnabled { get { return _options.Journal; } }

        /// <summary>
        /// Write original bytes page in a journal file (in sequence) - if journal not exists, create.
        /// </summary>
        public void WriteJournal(ICollection<byte[]> pages, uint lastPageID)
        {
            // write journal only if enabled
            if (_options.Journal == false) return;

            var size = BasePage.GetSizeOfPages(lastPageID + 1) +
                BasePage.GetSizeOfPages(pages.Count);

            _log.Write(Logger.JOURNAL, "extend datafile to journal - {0} pages", pages.Count);

            // set journal file length before write
            _stream.SetLength(size);

            // go to initial file position (after lastPageID)
            _stream.Seek(BasePage.GetSizeOfPages(lastPageID + 1), SeekOrigin.Begin);

            foreach(var buffer in pages)
            {
                // read pageID and pageType from buffer
                var pageID = BitConverter.ToUInt32(buffer, 0);
                var pageType = (PageType)buffer[PAGE_TYPE_POSITION];

                _log.Write(Logger.JOURNAL, "write page #{0:0000} :: {1}", pageID, pageType);

                // write page bytes
                _stream.Write(buffer, 0, BasePage.PAGE_SIZE);
            }

            _log.Write(Logger.JOURNAL, "flush journal to disk");

            // ensure all data are persisted in disk
            this.Flush();
        }

        /// <summary>
        /// Read journal file returning IEnumerable of pages
        /// </summary>
        public IEnumerable<byte[]> ReadJournal(uint lastPageID)
        {
            // position stream at begin journal area
            var pos = BasePage.GetSizeOfPages(lastPageID + 1);

            _stream.Seek(pos, SeekOrigin.Begin);

            var buffer = new byte[BasePage.PAGE_SIZE];

            while (_stream.Position < _stream.Length)
            {
                // read page bytes from journal file
                _stream.Read(buffer, 0, BasePage.PAGE_SIZE);

                yield return buffer;

                // now set position to next journal page
                pos += BasePage.PAGE_SIZE;

                _stream.Seek(pos, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Shrink datafile to crop journal area
        /// </summary>
        public void ClearJournal(uint lastPageID)
        {
            _log.Write(Logger.JOURNAL, "shrink datafile to remove journal area");

            this.SetLength(BasePage.GetSizeOfPages(lastPageID + 1));
        }

        /// <summary>
        /// Flush data from memory to disk
        /// </summary>
        public void Flush()
        {
            _log.Write(Logger.DISK, "flush data from memory to disk");

            _stream.Flush(_options.Flush);
        }

        #endregion


        #region Create Stream

        /// <summary>
        /// Create a new filestream. Can be synced over async task (netstandard)
        /// </summary>
        private FileStream CreateFileStream(string path, System.IO.FileMode mode, FileAccess access, FileShare share)
        {
            if (_options.Async)
            {
                return System.Threading.Tasks.Task.Run(() => new FileStream(path, mode, access, share, BasePage.PAGE_SIZE))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }

            return new FileStream(path, mode, access, share, 
                BasePage.PAGE_SIZE,
                System.IO.FileOptions.RandomAccess);
        }

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    public interface IDiskService : IDisposable
    {
        /// <summary>
        /// Open data file (creating if doest exists) and return header content bytes
        /// </summary>
        void Initialize(Logger log, string password);

        /// <summary>
        /// Read a page from disk datafile
        /// </summary>
        byte[] ReadPage(uint pageID);

        /// <summary>
        /// Write a page in disk datafile
        /// </summary>
        void WritePage(uint pageID, byte[] buffer);

        /// <summary>
        /// Set datafile length before start writing in disk
        /// </summary>
        void SetLength(long fileSize);

        /// <summary>
        /// Gets file length in bytes
        /// </summary>
        long FileLength { get; }


        /// <summary>
        /// Get if journal are enabled or not. Can optimize with has no jounal
        /// </summary>
        bool IsJournalEnabled { get; }

        /// <summary>
        /// Read journal file returning IEnumerable of pages
        /// </summary>
        IEnumerable<byte[]> ReadJournal(uint lastPageID);

        /// <summary>
        /// Write original bytes page in a journal file (in sequence) - if journal not exists, create.
        /// </summary>
        void WriteJournal(ICollection<byte[]> pages, uint lastPageID);

        /// <summary>
        /// Clear journal file
        /// </summary>
        void ClearJournal(uint lastPageID);

        /// <summary>
        /// Ensures all pages from the OS cache are persisted on medium
        /// </summary>
        void Flush();
        
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Implement generic Stream disk service. Used for any read/write/seek stream
    /// No journal implemented
    /// </summary>
    public class StreamDiskService : IDiskService
    {
        /// <summary>
        /// Position, on page, about page type
        /// </summary>
        private const int PAGE_TYPE_POSITION = 4;

        private Stream _stream;
        private Logger _log; // will be initialize in "Initialize()"
        private bool _disposeStream;

        #region Initialize disk

        public StreamDiskService(Stream stream, bool disposeStream = false)
        {
            _stream = stream;
            _disposeStream = disposeStream;
        }

        public void Initialize(Logger log, string password)
        {
            // get log instance to disk
            _log = log;

            // if stream are empty, create header page and save to stream
            if (_stream.Length == 0)
            {
                _log.Write(Logger.DISK, "initialize new datafile");

                // create datafile
                UltraLiteEngine.CreateDatabase(_stream, password);
            }
        }

        public virtual void Dispose()
        {
            if (_disposeStream)
            {
                _stream.Dispose();
            }
            else
            {
                // do nothing - keeps stream opened
            }
        }

        #endregion

        #region Read/Write

        /// <summary>
        /// Read page bytes from disk
        /// </summary>
        public virtual byte[] ReadPage(uint pageID)
        {
            var buffer = new byte[BasePage.PAGE_SIZE];
            var position = BasePage.GetSizeOfPages(pageID);

            // position cursor
            if (_stream.Position != position)
            {
                _stream.Seek(position, SeekOrigin.Begin);
            }

            // read bytes from data file
            _stream.Read(buffer, 0, BasePage.PAGE_SIZE);

            _log.Write(Logger.DISK, "read page #{0:0000} :: {1}", pageID, (PageType)buffer[PAGE_TYPE_POSITION]);

            return buffer;
        }

        /// <summary>
        /// Persist single page bytes to disk
        /// </summary>
        public virtual void WritePage(uint pageID, byte[] buffer)
        {
            var position = BasePage.GetSizeOfPages(pageID);

            _log.Write(Logger.DISK, "write page #{0:0000} :: {1}", pageID, (PageType)buffer[PAGE_TYPE_POSITION]);

            // position cursor
            if (_stream.Position != position)
            {
                _stream.Seek(position, SeekOrigin.Begin);
            }

            _stream.Write(buffer, 0, BasePage.PAGE_SIZE);
        }

        /// <summary>
        /// Set datafile length
        /// </summary>
        public void SetLength(long fileSize)
        {
            // fileSize parameter tell me final size of data file - helpful to extend first datafile
            _stream.SetLength(fileSize);
        }

        /// <summary>
        /// Returns file length
        /// </summary>
        public long FileLength { get { return _stream.Length; } }

        #endregion

        #region Not implemented in Stream

        /// <summary>
        /// No journal in Stream
        /// </summary>
        public bool IsJournalEnabled { get { return false; } }

        /// <summary>
        /// No journal implemented
        /// </summary>
        public void WriteJournal(ICollection<byte[]> pages, uint lastPageID)
        {
        }

        /// <summary>
        /// No journal implemented
        /// </summary>
        public IEnumerable<byte[]> ReadJournal(uint lastPageID)
        {
            yield break;
        }

        /// <summary>
        /// No journal implemented
        /// </summary>
        public void ClearJournal(uint lastPageID)
        {
        }


        /// <summary>
        /// No flush implemented
        /// </summary>
        public void Flush()
        {
        }

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Implement temporary disk access. Open datafile only when be used and delete when dispose. No journal, no sharing
    /// </summary>
    public class TempDiskService : IDiskService
    {
        /// <summary>
        /// Position, on page, about page type
        /// </summary>
        private const int PAGE_TYPE_POSITION = 4;

        private FileStream _stream;
        private string _filename;

        #region Initialize/Dispose disk

        public TempDiskService()
        {
        }

        public void Initialize(Logger log, string password)
        {
            // datafile will be created only when used
        }

        private void InternalInitialize()
        {
            // create a temp filename in temp directory
            _filename = Path.Combine(Path.GetTempPath(), "litedb-sort-" + Guid.NewGuid().ToString("n").Substring(0, 6) + ".db");

            // create disk
            _stream = this.CreateFileStream(_filename, System.IO.FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
        }

        public virtual void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;

                // after release stream, delete datafile
                FileHelper.TryDelete(_filename);
            }
        }

        #endregion

        #region Read/Write

        /// <summary>
        /// Read page bytes from disk
        /// </summary>
        public virtual byte[] ReadPage(uint pageID)
        {
            // if stream are not initialized but need header, create new header
            if(_stream == null && pageID == 0)
            {
                var header = new HeaderPage
                {
                    LastPageID = 1
                };

                return header.WritePage();
            }
            else if (_stream == null)
            {
                this.InternalInitialize();
            }

            var buffer = new byte[BasePage.PAGE_SIZE];
            var position = BasePage.GetSizeOfPages(pageID);

            // position cursor
            if (_stream.Position != position)
            {
                _stream.Seek(position, SeekOrigin.Begin);
            }

            // read bytes from data file
            _stream.Read(buffer, 0, BasePage.PAGE_SIZE);

            return buffer;
        }

        /// <summary>
        /// Persist single page bytes to disk
        /// </summary>
        public virtual void WritePage(uint pageID, byte[] buffer)
        {
            if (_stream == null) this.InternalInitialize();

            var position = BasePage.GetSizeOfPages(pageID);

            // position cursor
            if (_stream.Position != position)
            {
                _stream.Seek(position, SeekOrigin.Begin);
            }

            _stream.Write(buffer, 0, BasePage.PAGE_SIZE);
        }

        /// <summary>
        /// Set datafile length
        /// </summary>
        public void SetLength(long fileSize)
        {
            if (_stream == null) this.InternalInitialize();

            // fileSize parameter tell me final size of data file - helpful to extend first datafile
            _stream.SetLength(fileSize);
        }

        /// <summary>
        /// Returns file length
        /// </summary>
        public long FileLength { get { return _stream?.Length ?? 0; } }

        #endregion

        #region Journal file

        /// <summary>
        /// No journal
        /// </summary>
        public bool IsJournalEnabled { get { return false; } }

        /// <summary>
        /// No journal
        /// </summary>
        public void WriteJournal(ICollection<byte[]> pages, uint lastPageID)
        {
        }

        /// <summary>
        /// No journal
        /// </summary>
        public IEnumerable<byte[]> ReadJournal(uint lastPageID)
        {
            yield break;
        }

        /// <summary>
        /// No journal
        /// </summary>
        public void ClearJournal(uint lastPageID)
        {
        }

        /// <summary>
        /// Flush data from memory to disk
        /// </summary>
        public void Flush()
        {
            if (_stream != null)
            {
                _stream.Flush();
            }
        }

        #endregion


        #region Create Stream

        /// <summary>
        /// Create a new filestream. Can be synced over async task (netstandard)
        /// </summary>
        private FileStream CreateFileStream(string path, System.IO.FileMode mode, FileAccess access, FileShare share)
        {
            return System.Threading.Tasks.Task.Run(() => new FileStream(path, mode, access, share, BasePage.PAGE_SIZE))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        #endregion
    }
}

#pragma warning restore