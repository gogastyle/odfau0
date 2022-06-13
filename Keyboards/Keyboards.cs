using Telegram.Bot.Types.ReplyMarkups;
using PostgreSQL;

namespace Bot_Keyboards
{
    class Keyboards
    {
        public static InlineKeyboardMarkup InputToncoinKb= new(new []
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "↪️ Ввести TON кошелёк", callbackData: "input_toncoin_wallet"),
            },
        });

        public static InlineKeyboardMarkup NewToncoinWalletKb= new(new []
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "↪️ Изменить TON кошелёк", callbackData: "input_toncoin_wallet"),
            },
        });

        public static InlineKeyboardMarkup BuyNftKb= new(new []
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "💳 Купить NFT", callbackData: "buy_nft"),
            },
        });

        public static ReplyKeyboardMarkup MenuKb = new(new []
        {
            new KeyboardButton[] {"💎 Купить NFT", "🙊 Мои NFT"},
            new KeyboardButton[] {"💼 Мой Кошелек", "❓ Помощь"},
        })
        {
            ResizeKeyboard = true,
        };

        public static InlineKeyboardMarkup InfoKb = new(new []
        {
            new []
            {
                InlineKeyboardButton.WithUrl(text: "💭 Чат пресейла", url: "https://t.me/+zT2Fgh0M_hc4YmNk"),
            }
        });

        public static InlineKeyboardMarkup PaymentKb(long chatId)
        {
            string paymentLink = $"ton://transfer/{UserDB.GetTONWallet()}?amount={UserDB.GetAllInfo(chatId)[2]}000000000&text={UserDB.GetAllInfo(chatId)[3]}";
            InlineKeyboardMarkup paymentKb = new(new []
            {
                new[]
                {
                    InlineKeyboardButton.WithUrl(text: "Оплатить", url: $"{paymentLink}"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Проверить оплату", callbackData: "check_payment"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "⚙️ Отменить", callbackData: "cancel_payment"),
                },
            });
            return paymentKb;
        }

        public static InlineKeyboardMarkup CheckPaymentKb(long chatId)
        {
            string paymentLink = $"ton://transfer/{UserDB.GetTONWallet()}?amount={UserDB.GetAllInfo(chatId)[2]}000000000&text={UserDB.GetAllInfo(chatId)[3]}";
            InlineKeyboardMarkup checkPaymentKb = new(new []
            {
                new[]
                {
                    InlineKeyboardButton.WithUrl(text: "Оплатить", url: $"{paymentLink}"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Оплата проверяется...", callbackData: "3223"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "⚙️ Отменить", callbackData: "cancel_payment"),
                },
            });
            return checkPaymentKb;
        }
    }
}
