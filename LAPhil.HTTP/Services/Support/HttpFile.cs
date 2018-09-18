using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;


namespace LAPhil.HTTP
{

    public class HttpFile
    {
        public string Filename { get; set; }
        public string Name { get; set; }

        public string ContentType { get; set; } = "application/octect-stream";
        Stream _stream;
        byte[] _bytes;

        public HttpFile(FileStream stream, string filename = null, string name = null, string contentType = null)
        {
            Name = name;

            if (filename != null)
                Filename = filename;
            else
                Filename = Path.GetFileName(stream.Name);

            if (contentType != null)
                ContentType = contentType;

            _stream = stream;
        }

        public HttpFile(byte[] bytes, string filename, string name = null, string contentType = null)
        {
            Filename = filename;
            Name = name;

            if (contentType != null)
                ContentType = contentType;

            _bytes = bytes;
        }

        void prepareContent(HttpContent content)
        {
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = $"\"{Name}\"",
                FileName = $"\"{Filename}\""
            };

            content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
        }

        public HttpContent Content
        {
            get
            {
                if (_stream != null)
                    return StreamContent;
                else if (_bytes != null)
                    return ByteArrayContent;

                throw new ArgumentException("Not byte array or stream content set");
            }
        }

        public ByteArrayContent ByteArrayContent
        {
            get
            {
                var content = new ByteArrayContent(_bytes);
                prepareContent(content);
                return content;
            }
        }

        public StreamContent StreamContent
        {
            get
            {
                var content = new StreamContent(_stream);
                prepareContent(content);
                return content;
            }
        }
    }


}