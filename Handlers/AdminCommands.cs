using PostgreSQL;
using Bot_Keyboards;
using ConfigFile;

using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;


namespace CommandsSpace
{
    public static class Commands
    {
        async public static void AdminCommnads(ITelegramBotClient botClient, string messageText, long chatId, int messageId)
        {
            if(messageText == "/help")
            {
                string commandsText = "";
                foreach(var command in Config.AdminCommnads)
                {
                    commandsText += $"\n\n<code>{command.Key}</code> - {command.Value}";
                }
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "<b>🚀 Команды управления проектом: </b>" + commandsText,
                    parseMode: ParseMode.Html
                );
            }
            else if(messageText!.Contains("/get_total_nft"))
            {
                try
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"<b>Общее кол-во NFT: </b><code>{UserDB.GetTotalNftNumber()}</code>",
                        parseMode: ParseMode.Html
                    );
                }
                catch(Exception)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"❗️ Введите команду корректно.",
                        parseMode: ParseMode.Html
                    );
                }
            }
            else if(messageText!.Contains("/updade_total_nft "))
            {
                try
                {
                    string newTotalNft = messageText.Split(" ")[1];
                    UserDB.UpdateTotalNftNumber(newTotalNft);
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"<b>Общее кол-во NFT обновлено на: </b><code>{newTotalNft}</code>",
                        parseMode: ParseMode.Html
                    );
                }
                catch(Exception)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"❗️ Введите команду корректно.",
                        parseMode: ParseMode.Html
                    );
                }
            }
            else if(messageText!.Contains("/add_to_total_nft "))
            {
                try
                {
                    string newTotalNft = messageText.Split(" ")[1];
                    UserDB.AddTotalNftNumber(newTotalNft);
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"<b>Общее кол-во NFT обновлено на: </b><code>{UserDB.GetTotalNftNumber()}</code>",
                        parseMode: ParseMode.Html
                    );
                }
                catch(Exception)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"❗️ Введите команду корректно.",
                        parseMode: ParseMode.Html
                    );
                }
            }
            else if(messageText!.Contains("/update_ton_wallet "))
            {
                try
                {
                    string newTONWallet = messageText.Split(" ")[1];
                    UserDB.UpdateTONWallet(newTONWallet);
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"<b>TON кошелёк обновлен на: </b><code>{UserDB.GetTONWallet()}</code>",
                        parseMode: ParseMode.Html
                    );
                }
                catch(Exception)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"❗️ Введите команду корректно.",
                        parseMode: ParseMode.Html
                    );
                }
            }
            else if(messageText!.Contains("/get_ton_wallet"))
            {
                try
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"<b>TON кошелёк: </b><code>{UserDB.GetTONWallet()}</code>",
                        parseMode: ParseMode.Html
                    );
                }
                catch(Exception)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"❗️ Введите команду корректно.",
                        parseMode: ParseMode.Html
                    );
                }
            }
        }
    }
}