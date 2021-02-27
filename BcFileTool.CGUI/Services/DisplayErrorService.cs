using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BcFileTool.CGUI.Services
{
    public class DisplayErrorService
    {
        public void ShowException(Exception exception)
        {
            var answer = MessageBox.ErrorQuery("Error", exception.Message, "Ok", "Details");
            if(answer == 1)
            {
                var message = DumpException(exception);
                MessageBox.Query("Error details", message, "Ok");
            }
        }

        private string DumpException(Exception exception)
        {
            StringBuilder messageBuilder = new StringBuilder();
            string indent = "";
            do
            {
                messageBuilder.AppendLine($"{indent}{exception.Message}");
                messageBuilder.AppendLine();
                foreach (var line in exception.StackTrace.Split(Environment.NewLine))
                {
                    messageBuilder.AppendLine($"{indent}{line}");
                }
                messageBuilder.AppendLine();
                indent += "   ";
                exception = exception.InnerException;
            } while (exception != null);

            return messageBuilder.ToString();
        }
    }
}
