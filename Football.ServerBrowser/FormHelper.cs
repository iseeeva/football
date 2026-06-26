using Football.Serialization;
using Football.ServerBrowser.Messages;

namespace Football.ServerBrowser
{
    public enum TextTheme
    {
        Green,
        Red,
        Orange,
        TextMuted
    }

    public static class FormHelper
    {
        public static async Task ExecuteRequestAsync<TResponse>(
            Func<Task<IMessage>> requestFunc,
            Action<TResponse> onSuccess,
            Action<Exception>? onExpection = null,
            Action? onFinally = null,
            Label? statusLabel = null) where TResponse : class, IMessage
        {
            try
            {
                var response = await requestFunc();

                if (response is BrowserStatusResponseMessage status)
                {
                    RenderColoredText(statusLabel, (status.Status ? "✓ " : "✗ ") + status.StatusMessage,
                        status.Status ? TextTheme.Green : TextTheme.Red);

                    if (status.Status) onSuccess(response as TResponse);
                }
                else if (response is TResponse typedResponse)
                {
                    onSuccess(typedResponse);
                }
            }
            catch (Exception ex)
            {
                RenderColoredText(statusLabel, $"Error: {ex.Message}", TextTheme.Red);
                onExpection?.Invoke(ex);
            }
            finally
            {
                onFinally?.Invoke();
            }
        }

        public static void RenderColoredText(Label label, string targetText, TextTheme style)
        {
            label.Text = targetText;
            label.ForeColor = style switch
            {
                TextTheme.Green => Color.MediumSeaGreen,
                TextTheme.Red => Color.Crimson,
                TextTheme.Orange => Color.DarkOrange,
                _ => Color.DarkGray
            };
        }
    }
}
