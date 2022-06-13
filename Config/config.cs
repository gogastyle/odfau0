namespace ConfigFile
{
    public class Config
    {
        public static string botToken = "5240465996:AAFKgcT9QSxq7G8l4SdSKZKqV1mCgsxxyjk";
        public static long adminChannel = -1001693620959;

        public static string walletSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-";

        // Админ команды
        public static Dictionary<string, string> AdminCommnads = new Dictionary<string, string>()
        {
            {"/get_total_nft", "посмотреть общее кол-во NFT"},
            {"/updade_total_nft кол-во", "обновить общее кол-во NFT"},
            {"/add_to_total_nft кол-во", "добавить к общему кол-во NFT"},
            {"/update_ton_wallet кошелёк", "обновить TON кошелёк"},
            {"/get_ton_wallet", "посмотреть TON кошелёк"},
        };
    }
}