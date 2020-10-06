using PhotosCategorier.Photo;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace PhotosCategorier.Render
{
    public class RenderPool : IRenderer
    {
        private readonly PhotographsGenerator Generator;
        public RenderPool(PhotographsGenerator photographsGenerator)
        {
            Generator = photographsGenerator;
        }

        private Task<Bitmap> RenderNextTask;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public Bitmap GetCurrentRender()
        {
            Bitmap current;
            try
            {
                RenderNextTask.Wait();
                current = RenderNextTask.Result;
            }
            catch
            {
                throw;
            }
            finally
            {
                RenderNextTask = Task.Factory.StartNew(() => { return RenderNext(); });
            }
            try
            {
                return current;
            }
            finally
            {
                current = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CannotProcessImageException"></exception>
        /// <exception cref="CannotOpenFileException"></exception>
        public Bitmap Init()
        {
            Bitmap current;
            try
            {
                current = Generator.Current.GetImageSource();
            }
            catch
            {
                throw;
            }
            finally
            {
                RenderNextTask = Task.Factory.StartNew(() => { return RenderNext(); });
            }
            try
            {
                return current;
            }
            finally
            {
                current = null;
            }
        }

        private Bitmap RenderNext()
        {

            try
            {
                var photo = Generator.Peek();
                if (photo == null)
                {
                    throw new GeneratorIsEmptyException();
                }
                return photo.GetImageSource();
            }
            catch
            {
                throw;
            }
        }

        [Serializable]
        public class GeneratorIsEmptyException : Exception
        {
            public GeneratorIsEmptyException() { }
            public GeneratorIsEmptyException(string message) : base(message) { }
            public GeneratorIsEmptyException(string message, Exception inner) : base(message, inner) { }
            protected GeneratorIsEmptyException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}
