using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tiberhealth.XsvSerializer.Extensions;

namespace Tiberhealth.XsvSerializer
{
    /// <summary>
    /// Reader to transforms/deserialize an Xsv file stream to objects.
    /// </summary>
    public abstract class XsvReader
    {
        protected char Delimiter { get; set; }

        private Stream SourceStream { get; }
        private int BufferSize { get; }
        private byte[] _buffer;

        private int _bufferPos =>
            this._currentPosition < this.BufferSize
                ? (int)this._currentPosition
                : Convert.ToInt32(this._currentPosition % this.BufferSize);

        protected long _currentPosition = 0;
        protected long _dataPosition = 0;

        protected long CurrentPosition => this._currentPosition;

        protected XsvReader(Stream stream, int bufferSize = 10)
        {
            this.SourceStream = stream;
            this.BufferSize = bufferSize;

            this._buffer = new byte[this.BufferSize];
            this._dataPosition = 0;
        }

        /// <summary>
        /// Sets the position of the data elements (right after the header)
        /// </summary>
        /// <param name="position">The position to set it, or NULL to set it at the current position.</param>
        /// <returns>XsvReader object for chaining</returns>
        protected XsvReader SetDataPosition(long? position = null)
        {
            this._dataPosition = position ?? this._currentPosition;
            return this;
        }

        /// <summary>
        /// Main logic for getting the next line in the XSV file
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetNextLine()
        {
            var stringParts = new List<string>();
            var bytes = new List<byte>();
            var inQuotes = false;
            char? beginQuote = null;
            bool eol = false;
            while (!eol)
            {
                if (this._currentPosition >= this.SourceStream.Length) break;
                if (this._bufferPos == 0) this.LoadBuffer();
                var currentByte = (char)this._buffer[this._bufferPos];
                this._currentPosition += 1;

                switch (currentByte)
                {
                    case '"':
                        if (inQuotes && beginQuote == currentByte)
                        {
                            beginQuote = null;
                            inQuotes = false;
                            break;
                        }

                        inQuotes = true;
                        beginQuote = currentByte;
                        break;

                    case '\n':
                        if (!inQuotes)
                        {
                            eol = true;
                            break;
                        }

                        bytes.Add((byte)currentByte);
                        break;

                    case '\r':
                        if (inQuotes) bytes.Add((byte)currentByte);
                        break;

                    default:
                        if (currentByte == this.Delimiter && !inQuotes)
                        {
                            stringParts.Add(Encoding.UTF8.GetString(bytes.ToArray()));
                            bytes.Clear();
                            break;
                        }

                        bytes.Add((byte)currentByte);
                        break;
                }
            }

            if (bytes.Any()) stringParts.Add(Encoding.UTF8.GetString(bytes.ToArray()));
            return stringParts;
        }

        private void LoadBuffer()
        {
            if (this._currentPosition != this.SourceStream.Position)
                this.SourceStream.Position =
                    Convert.ToInt64(
                        Math.Floor(Convert.ToDecimal(this._currentPosition) / Convert.ToDecimal(this.BufferSize)) *
                        Convert.ToDecimal(this.BufferSize));

            var result = this.SourceStream.Read(this._buffer);
            if (result < this.BufferSize)
            {
                return;
            }

            if (_currentPosition < 0) this._currentPosition = 0;
        }

        internal void Reset()
        {
            this._currentPosition = this._dataPosition;
            this.LoadBuffer();
        }
    }

    /// <summary>
    /// Reader to transforms/deserialize an Xsv file stream to objects.
    /// </summary>
    /// <typeparam name="TResult">The resulting object </typeparam>
    public class XsvReader<TResult> : XsvReader, IEnumerable<TResult>
        where TResult : class, new()
    {

        private readonly Mapper<TResult> _mapper;

        /// <summary>
        /// Constructor for the deserialization reader
        /// </summary>
        /// <param name="stream">Stream containing the XSV Information</param>
        /// <param name="bufferSize"></param>
        public XsvReader(Stream stream, char? delimiter = null, int bufferSize = 10) : base(stream, bufferSize)
        {
            this.Delimiter = delimiter ?? this.DetermineDelimiter();

            this._mapper = new Mapper<TResult>(this);
            this.SetDataPosition(); 
        }

        /// <summary>
        /// The enumerator to transverse the data and objects of the CSV file.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TResult> GetEnumerator() => new XsvReaderEnumerator<TResult>(this);
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Gets the next object based on the position of the stream
        /// </summary>
        /// <returns>The generated object based on the stream position.</returns>
        internal TResult GetNext()
        {
            var xsvLine = this.GetNextLine();
            return xsvLine.Any() ? this._mapper.MapObject(xsvLine, out var warnings) : null;
        }

        private char DetermineDelimiter()
        {
            if (typeof(TResult).HasCustomAttribute<XsvObjectAttribute>(out var attribute)) return attribute.Delimiter;

            return ',';
        }

    }
}
