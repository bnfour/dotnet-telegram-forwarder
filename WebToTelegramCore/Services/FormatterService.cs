using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Class that converts Markdown to HTML.
    /// </summary>
    public class FormatterService : IFormatterService
    {
        /// <summary>
        /// Transforms Markdown to HTML. Expects Markdown like Telegram clients do:
        /// **bold** and __italic__ are main differences from the CommonMark.
        /// </summary>
        /// <param name="TgFlavoredMarkdown">Markdown-formatted text.</param>
        /// <returns>Text with same format but in HTML supported by Telegram's API.</returns>
        public string TransformToHtml(string TgFlavoredMarkdown)
        {
            throw new NotImplementedException();
        }
    }
}
