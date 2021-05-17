using System;
using System.Linq;
using System.Text;
using Terminal.Gui;

namespace BcFileTool.CGUI.Services
{
    public class DisplayService
    {
        public void ShowException(Exception exception)
        {
            var answer = MessageBox.ErrorQuery("Error", exception.Message+"\n", "Ok", "Details");
            if (answer == 1)
            {
                var message = DumpException(exception);
                MessageBox.Query("Error details", message, "Ok");
            }
        }

        public bool ShowConfirmation(string message)
        {
            var answer = MessageBox.Query("Confirm", message + "\n", "Ok", "Cancel");
            if (answer == 0)
            {
                return true;
            }
            return false;
        }

        public string DirectoryDialog(string title, string message)
        {
            using (var fileDialog = new OpenDialog(title, message)
            {
                CanChooseFiles = false,
                CanChooseDirectories = true,
                CanCreateDirectories = false
            })
            {
                Application.Run(fileDialog);

                if (fileDialog.Canceled)
                {
                    return null;
                }

                return fileDialog.DirectoryPath.ToString();
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
