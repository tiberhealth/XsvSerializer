using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tiberhealth.XsvSerializer
{
    public class XsvReader<TResult> : IEnumerable<TResult>
        where TResult : class, new()
    {
        private Stream SourceStream { get; }

        private int BufferSize { get; }

        private long _currentPosition = 0;
        private long _dataPosition = 0;

        private byte[] _buffer;
        private char _delimiter = ',';

        private int _bufferPos =>
            this._currentPosition < this.BufferSize
                ? (int) this._currentPosition
                : Convert.ToInt32(this._currentPosition % this.BufferSize);

        public XsvReader(Stream stream, int bufferSize = 10)
        {
            this.SourceStream = stream;
            this.BufferSize = bufferSize;

            this._buffer = new byte[this.BufferSize];

            var header = this.FirstOrDefault();
            this._dataPosition = this._currentPosition; // After getting the header - save the position
        }

        public IEnumerator<TResult> GetEnumerator() => new XsvReaderEnumerator<TResult>(this);
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        internal void Reset()
        {
            this._currentPosition = this._dataPosition;
            this.LoadBuffer();
        }

        internal TResult GetNext()
        {
            var xsvLine = this.GetNextLine();
            return xsvLine.Any() ? new TResult() : null;
        }

        private IEnumerable<string> GetNextLine()
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
                var currentByte = (char) this._buffer[this._bufferPos];
                this._currentPosition += 1;

                switch (currentByte)
                {
                    case '\'':
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

                        bytes.Add((byte) currentByte);
                        break;

                    case '\r':
                        if (inQuotes) bytes.Add((byte) currentByte);
                        break;

                    default:
                        if (currentByte == this._delimiter)
                        {
                            stringParts.Add(Encoding.UTF8.GetString(bytes.ToArray()));
                            bytes.Clear();
                            break;
                        }

                        bytes.Add((byte) currentByte);
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
    }
}