using PostgreSQL;
using Bot_Keyboards;
// using Payments;
using ConfigFile;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;



namespace CallbackHandler
{
    public static class CallHandler
    {
        public static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            try
            {
                long chatId = callbackQuery.Message!.Chat.Id;
                int messageId = callbackQuery.Message.MessageId;
                string? firstName = callbackQuery.Message.Chat.FirstName;
                string? username = callbackQuery.Message.Chat.Username;

                switch(callbackQuery.Data)
                {
                    case "input_toncoin_wallet":
                        UserDB.UpdateState(chatId, "Wallet");
                        await botClient.EditMessageTextAsync(
                            chatId: chatId,
                            messageId: messageId,
                            text: "<b>⤵️ Введите адрес вашего TON кошелька: </b>",
                            parseMode: ParseMode.Html
                        );
                        return;
                    case "buy_nft":
                        UserDB.UpdateState(chatId, "NumberOfNft");
                        await botClient.EditMessageTextAsync(
                            chatId: chatId,
                            messageId: messageId,
                            text: "<b>⤵️ Введите количество NFT: </b>",
                            parseMode: ParseMode.Html
                        );
                        return;
                    case "cancel_payment":
                        UserDB.UpdateState(chatId, "MainMenu");
                        await botClient.SendTextMessageAsync(
                            chatId: Config.adminChannel,
                            text: $"<b>Пользователь @{username}, отменил оплату.</b>",
                            parseMode: ParseMode.Html
                        );
                        await botClient.EditMessageTextAsync(
                            chatId: chatId,
                            messageId: messageId,
                            text: $"<b>Цена NFT во время PRESALE I:\n\nпри покупке от 1 NFT:</b> <code>49 TON / 1 шт</code>\n<b>при покупке от 3 NFT:</b> <code>44 TON / 1 шт</code>\n<b>при покупке от 5 NFT:</b> <code>39 TON / 1 шт</code>\n<b>при покупке от 10 NFT:</b> <code>34 TON / 1 шт</code>\n<b>при покупке от 15 NFT:</b> <code>29 TON / 1 шт</code>\n\n<b>Доступно во время PRESALE I: {UserDB.GetTotalNftNumber()} NFT</b>",
                            parseMode: ParseMode.Html,
                            replyMarkup: Keyboards.BuyNftKb
                        );
                        return;
                    case "check_payment":
                        await botClient.EditMessageReplyMarkupAsync(
                            chatId: chatId,
                            messageId: messageId,
                            replyMarkup: Keyboards.CheckPaymentKb(chatId)
                        );
                        return;
                }
            }
            catch(Exception){ return; }
        }
    }            
}