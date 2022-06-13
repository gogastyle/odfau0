using PostgreSQL;
using Bot_Keyboards;
using ConfigFile;
using CommandsSpace;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

using System;
using System.Text.RegularExpressions;
using System.Globalization;

// States
// using States;

namespace MessageHandler
{
    public static class MessHandler
    {
        public static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            try
            {
                long chatId = message.Chat.Id;
                string? messageText = message.Text;
                int messageId = message.MessageId;
                string? username = message.Chat.Username;
                string? firstName = message.Chat.FirstName;

                if(messageText![0]=='/')
                {
                    if(messageText!.Contains("/start"))
                    {   
                        if(chatId.ToString()[0]=='-'){ return; }
                        else if(UserDB.CheckUser(chatId))
                        {
                            if(UserDB.GetWallet(chatId)=="Не указан")
                            {
                                await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: $"Введите адрес своего <b>TON</b> кошелька (прим. Tonkeeper, Toncoin)\n\nАдрес кошелька нужен, чтобы в дальнейшем безошибочно закрепить за ним купленные NFT.",
                                    parseMode: ParseMode.Html,
                                    replyMarkup: Keyboards.InputToncoinKb
                                );
                            }
                            else
                            {
                                UserDB.UpdateState(chatId, "MainMenu");
                                await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: $"<b>PRESALE TON DOODLES...<a href=\"https://t.me/TonDoodlesNFT\">💎</a>\n\n Наш Канал: </b>@TonDoodlesNFT",
                                    parseMode: ParseMode.Html,
                                    replyMarkup: Keyboards.MenuKb
                                );
                            }
                            return;
                        }
                        else
                        {
                            try
                            {
                                UserDB.CreateUser(chatId, username!);
                                await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: $"<b>PRESALE TON DOODLES...<a href=\"https://t.me/TonDoodlesNFT\">💎</a>\n\n Наш Канал: </b>@TonDoodlesNFT",
                                    parseMode: ParseMode.Html
                                );
                                
                                await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: $"<b>💎 Для работы с ботом нужно указать Ваш текущий TON кошелёк.</b>",
                                    parseMode: ParseMode.Html,
                                    replyMarkup: Keyboards.InputToncoinKb
                                );
                            }
                            catch(Exception)
                            {
                                await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: $"<b>💎 Для работы с ботом нужно указать @username.</b>",
                                    parseMode: ParseMode.Html
                                );
                            }
                            return;
                        }
                    }
                    else
                    {
                        // Админ команды
                        if(chatId==Config.adminChannel)
                        {
                            Commands.AdminCommnads(botClient, messageText!, chatId, messageId);
                            return;
                        }
                        else if(CheckSubChannel(botClient.GetChatMemberAsync(Config.adminChannel, chatId).Result.Status.ToString()))
                        {
                            Commands.AdminCommnads(botClient, messageText!, chatId, messageId);
                            return;
                        }
                    }
                }


                switch(messageText)
                {
                    case "💼 Мой Кошелек":
                        UserDB.UpdateState(chatId, "MainMenu");
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: $"<b>💼Кошелек TON</b>\n\n<b>Аадрес:</b>\n<code>{UserDB.GetWallet(chatId)}</code>",
                            parseMode: ParseMode.Html,
                            replyMarkup: Keyboards.NewToncoinWalletKb
                        );
                        return;
                    case "💎 Купить NFT":

                        UserDB.UpdateState(chatId, "MainMenu");
                        if(UserDB.GetWallet(chatId)=="Не указан")
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: $"📝 Для покупки NFT введите адрес своего <b>TON</b> кошелька (прим. Tonkeeper, Toncoin Wallet)\n\nАдрес кошелька нужен, чтобы в дальнейшем безошибочно закрепить за ним купленные NFT.",
                                parseMode: ParseMode.Html,
                                replyMarkup: Keyboards.InputToncoinKb
                            );
                            return;
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: $"<b>💎 Цена NFT во время PRESALE 2:</b>\n\nПри покупке от 1 NFT: <code>59 TON / 1 шт</code>\nПри покупке от 3 NFT: <code>54 TON / 1 шт</code>\nПри покупке от 5 NFT: <code>49 TON / 1 шт</code>\nПри покупке от 10 NFT: <code>44 TON / 1 шт</code>\nПри покупке от 15 NFT: <code>39 TON / 1 шт</code>\n\nДля покупки доступно: <b>{UserDB.GetTotalNftNumber()} из 1111 NFT</b> по цене от 39 TON",
                                parseMode: ParseMode.Html,
                                replyMarkup: Keyboards.BuyNftKb
                            );
                            return;
                        }
                    case "❓ Помощь":
                        UserDB.UpdateState(chatId, "MainMenu");
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: $"<b>🤕Вам необходима помощь?</b>\nЕсли у Вас возникли проблемы во время покупки, или Вы хотите задать вопросы касаемо пользования ботом - смело связывайтесь с нами!\n\n<b>💎Наш Контакт:</b> @MerIinBodro",
                            parseMode: ParseMode.Html,
                            replyMarkup: Keyboards.InfoKb
                        );
                        return;
                    case "🙊 Мои NFT":
                        UserDB.UpdateState(chatId, "MainMenu");
                        string purchasedNft = UserDB.GetParsedNftCount(chatId);
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: $"<b>🙈 Пользователь: </b>@{username}\n\n<b>Куплено во время действующего Сейла: </b>{purchasedNft}\n<b>Куплено Всего: </b>{purchasedNft}<b>💎ROUND CLASS:</b> 0\n<b>💎SIMS CLASS:</b> 0\n<b>💎EMERALD CLASS:</b> 0\n<b>💎MYTHICAL CLASS:</b> 0",
                            parseMode: ParseMode.Html
                        );
                        return;
                }

                switch (UserDB.state(chatId))
                {
                    case "MainMenu":
                        return;
                    case "Wallet":
                        string wallet = messageText;
                        if(CheckWalletRight(wallet))
                        {
                            if(UserDB.GetWallet(chatId)=="Не указан")
                            {
                                if(UserDB.CheckWallet(wallet))
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatId,
                                        text: $"<b>Данный TON кошелек уже привязан к другому аккаунту…\nПожалуйста воспользуетесь кошельком, которые еще не был привязан к нашему боту!\n\n⤵️ Введите адрес вашего кошелька:</b>",
                                        parseMode: ParseMode.Html
                                    );
                                    return;
                                }
                                else
                                {
                                    UserDB.UpdateWallet(chatId, wallet);
                                    UserDB.UpdateState(chatId, "MainMenu");
                                
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatId,
                                        text: $"<b>💎 Ваш текущий TON адрес: </b><code>{wallet}</code>",
                                        parseMode: ParseMode.Html,
                                        replyMarkup: Keyboards.MenuKb
                                    );
                                    
                                    await botClient.SendTextMessageAsync(
                                        chatId: Config.adminChannel,
                                        text: $"<b>Пользователь @{username} запустил бота.</b>\n\n<b>🆔 ID: </b><code>{chatId}</code>\n<b>💼 Кошелёк: </b><code>{wallet}</code>",
                                        parseMode: ParseMode.Html
                                    );
                                    return;
                                }
                            }
                            else
                            {
                                if(UserDB.CheckWallet(wallet))
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatId,
                                        text: $"<b>Данный TON кошелек уже привязан к другому аккаунту…\nПожалуйста воспользуетесь кошельком, которые еще не был привязан к нашему боту!\n\n⤵️ Введите адрес вашего кошелька:</b>",
                                        parseMode: ParseMode.Html
                                    );
                                    return;
                                }
                                else
                                {
                                    UserDB.UpdateWallet(chatId, wallet);
                                    UserDB.UpdateState(chatId, "MainMenu");
                                
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatId,
                                        text: $"<b>💎 Ваш текущий TON адрес: </b><code>{wallet}</code>",
                                        parseMode: ParseMode.Html,
                                        replyMarkup: Keyboards.MenuKb
                                    );
                                    await botClient.SendTextMessageAsync(
                                        chatId: Config.adminChannel,
                                        text: $"<b>Пользователь @{username} изменил свой кошелёк.</b>\n\n<b>🆔 ID: </b><code>{chatId}</code>\n<b>💼 Кошелёк: </b><code>{wallet}</code>",
                                        parseMode: ParseMode.Html
                                    );
                                    return;
                                }
                            }
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: $"<b>☠️ Ошибка при вводе данных. Введите адрес вашего TON кошелька корректно.\n\n Как должен выглядеть кошелёк? </b> <code>EQAXFiR6KO1YJmiNOnlRzrkJUQARVU-audCjU53PCD4GR_93</code>",
                                parseMode: ParseMode.Html,
                                replyMarkup: Keyboards.MenuKb
                            );
                        }
                        return;
                    case "NumberOfNft":
                        if(int.TryParse(messageText, out int number) && Int32.Parse(messageText) > 0)
                        {
                            int nftNumber = Int32.Parse(messageText);
                            if(Int32.Parse(UserDB.GetTotalNftNumber()) >= nftNumber)
                            {
                                UserDB.UpdateNftNumber(chatId, nftNumber);
                                GenerateIdentifier(chatId);
                                
                                await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: GetTotalPrice(chatId, nftNumber),
                                    parseMode: ParseMode.Html,
                                    replyMarkup: Keyboards.PaymentKb(chatId)
                                );
                                
                                await botClient.SendTextMessageAsync(
                                    chatId: Config.adminChannel,
                                    text: $"<b>Пользователь @{username} cоздал заявку на покупку.</b>\n\n<b>Кол-во NFT: </b><code>{UserDB.GetAllInfo(chatId)[1]}</code>\n<b>Общая цена: </b><code>{UserDB.GetAllInfo(chatId)[2]} TON</code>\n<b>Идентификатор: </b><code>{UserDB.GetAllInfo(chatId)[3]}</code>",
                                    parseMode: ParseMode.Html
                                );
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: $"<b>☠️ Ошибка при вводе данных. Осталось всего {UserDB.GetTotalNftNumber()} NFT. Введите количество NFT повторно.</b>",
                                    parseMode: ParseMode.Html
                                );
                            }
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "<b>☠️ Ошибка при вводе данных. Количество NFT должно быть числом и должно превышать 0. Введите количество NFT корректно.</b>",
                                parseMode: ParseMode.Html
                            );
                        }
                        return;
                }
            }
            catch(Exception e){ return; }
        }

        private static string GetTotalPrice(long chatId, int nftNumber)
        {
            int price;
            int totalPrice;
            string text;
            if(nftNumber >= 1 && nftNumber < 3)
            {
                price = 59;
                totalPrice = price * nftNumber;
                UserDB.UpdateTotalPrice(chatId, totalPrice);
                text = $"<b>📎 Создана заявка на покупку {nftNumber} NFT.</b>\n\n<b>Курс покупки: </b><code>{price} TON / 1 шт.</code>\n<b>Сумма к оплате: </b><code>{totalPrice}</code><b> TON.</b>\n\n<b>Для совершения оплаты нажмите на кнопку <b>Оплатить</b> или сделайте перевод по указанным реквизитам:</b>\n<b>Адрес кошелька: </b><code>{UserDB.GetTONWallet()}</code>\n<b>Комментарий: </b><code>{UserDB.GetAllInfo(chatId)[3]}</code>";
                return text;
            }
            else if(nftNumber >= 3 && nftNumber < 5)
            {
                price = 54;
                totalPrice = price * nftNumber;
                UserDB.UpdateTotalPrice(chatId, totalPrice);
                text = $"<b>📎 Создана заявка на покупку {nftNumber} NFT.</b>\n\n<b>Курс покупки: </b><code>{price} TON / 1 шт.</code>\n<b>Сумма к оплате: </b><code>{totalPrice}</code><b> TON.</b>\n\n<b>Для совершения оплаты нажмите на кнопку <b>Оплатить</b> или сделайте перевод по указанным реквизитам:</b>\n<b>Адрес кошелька: </b><code>{UserDB.GetTONWallet()}</code>\n<b>Комментарий: </b><code>{UserDB.GetAllInfo(chatId)[3]}</code>";
                return text;
            }
            else if(nftNumber >= 5 && nftNumber < 10)
            {
                price = 49;
                totalPrice = price * nftNumber;
                UserDB.UpdateTotalPrice(chatId, totalPrice);
                text = $"<b>📎 Создана заявка на покупку {nftNumber} NFT.</b>\n\n<b>Курс покупки: </b><code>{price} TON / 1 шт.</code>\n<b>Сумма к оплате: </b><code>{totalPrice}</code><b> TON.</b>\n\n<b>Для совершения оплаты нажмите на кнопку <b>Оплатить</b> или сделайте перевод по указанным реквизитам:</b>\n<b>Адрес кошелька: </b><code>{UserDB.GetTONWallet()}</code>\n<b>Комментарий: </b><code>{UserDB.GetAllInfo(chatId)[3]}</code>";
                return text;
            }
            else if(nftNumber >= 10 && nftNumber < 15)
            {
                price = 44;
                totalPrice = price * nftNumber;
                UserDB.UpdateTotalPrice(chatId, totalPrice);
                text = $"<b>📎 Создана заявка на покупку {nftNumber} NFT.</b>\n\n<b>Курс покупки: </b><code>{price} TON / 1 шт.</code>\n<b>Сумма к оплате: </b><code>{totalPrice}</code><b> TON.</b>\n\n<b>Для совершения оплаты нажмите на кнопку <b>Оплатить</b> или сделайте перевод по указанным реквизитам:</b>\n<b>Адрес кошелька: </b><code>{UserDB.GetTONWallet()}</code>\n<b>Комментарий: </b><code>{UserDB.GetAllInfo(chatId)[3]}</code>";
                return text;
            }
            else if(nftNumber >= 15)
            {
                price = 39;
                totalPrice = price * nftNumber;
                UserDB.UpdateTotalPrice(chatId, totalPrice);
                text = $"<b>📎 Создана заявка на покупку {nftNumber} NFT.</b>\n\n<b>Курс покупки: </b><code>{price} TON / 1 шт.</code>\n<b>Сумма к оплате: </b><code>{totalPrice}</code><b> TON.</b>\n\n<b>Для совершения оплаты нажмите на кнопку <b>Оплатить</b> или сделайте перевод по указанным реквизитам:</b>\n<b>Адрес кошелька: </b><code>{UserDB.GetTONWallet()}</code>\n<b>Комментарий: </b><code>{UserDB.GetAllInfo(chatId)[3]}</code>";
                return text;
            }
            else
            {
                return "";
            }
        }

        private static void GenerateIdentifier(long chatId)
        {
            char[] letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_".ToCharArray();

            Random rand = new Random();

            // Делаем слова.
            string identifier = "";
            for (int i = 1; i <= 15; i++)
            {
                int letterNum = rand.Next(0, letters.Length - 1);
                identifier += letters[letterNum];
            }
            UserDB.UpdateIdentifier(chatId, identifier);
        }

        private static bool CheckWalletRight(string wallet)
        {
            if(wallet.Length==48)
            {
                foreach(var letter in wallet)
                {
                    if(Config.walletSymbols.Contains(letter)){  }
                    else{return false;}
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckSubChannel(string chatMember)
        {
            if(chatMember != "Left")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
