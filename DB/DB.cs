using Npgsql;

using System.Globalization;
namespace PostgreSQL
{
    public static class UserDB
    {
        private static string user_db_connection = DBConfig.user_db;

        public static void TruncateTable()
        {
            using var con = new NpgsqlConnection(user_db_connection);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            cmd.CommandText = $"TRUNCATE TABLE users_table";
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // public static List<string> GetAllWallets()
        // {
        //     List<string> allWallets= new List<string>();
        //     using var con = new NpgsqlConnection(user_db_connection);
        //     con.Open();
        //     var sql = $"SELECT wallet FROM users_table";
        //     using var cmd = new NpgsqlCommand(sql, con);
        //     cmd.ExecuteNonQuery();
        //     using (NpgsqlDataReader reader = cmd.ExecuteReader())
        //     {
        //         while (reader.Read())
        //         {
        //             allWallets.add(reader.GetString(0));
        //         }
        //         con.Close();
        //         return allWallets;
        //     }    
        // }



        // Получить ton кошелек
        public static string GetTONWallet()
        {
            using var con = new NpgsqlConnection(user_db_connection);
            con.Open();
            var sql = $"SELECT ton_wallet FROM nft_information";
            using var cmd = new NpgsqlCommand(sql, con);
            var result = cmd.ExecuteScalar();
            return result!.ToString()!;
        }

        // Обновить ton кошелек
        public static void UpdateTONWallet(string TONWallet)
        {
            using (var conn = new NpgsqlConnection(user_db_connection))
            {
                
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE nft_information SET ton_wallet = @q", conn))
                {
                    command.Parameters.AddWithValue("q", TONWallet);
                    int nRows = command.ExecuteNonQuery();
                }
            }
        }

        // Получить кол-во NFT
        public static string GetTotalNftNumber()
        {
            using var con = new NpgsqlConnection(user_db_connection);
            con.Open();
            var sql = $"SELECT number_of_nft FROM nft_information";
            using var cmd = new NpgsqlCommand(sql, con);
            var result = cmd.ExecuteScalar();
            return result!.ToString()!;
        }

        // Обновить кол-во NFT
        public static void UpdateTotalNftNumber(string nftNumber)
        {
            using (var conn = new NpgsqlConnection(user_db_connection))
            {
                
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE nft_information SET number_of_nft = @q", conn))
                {
                    command.Parameters.AddWithValue("q", nftNumber);
                    int nRows = command.ExecuteNonQuery();
                }
            }
        }

        // Добавить кол-во NFT
        public static void AddTotalNftNumber(string nftNumber)
        {
            int newTotalNftNumber = Int32.Parse(GetTotalNftNumber()) + Int32.Parse(nftNumber);
            using (var conn = new NpgsqlConnection(user_db_connection))
            {
                
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE nft_information SET number_of_nft = @q", conn))
                {
                    command.Parameters.AddWithValue("q", newTotalNftNumber);
                    int nRows = command.ExecuteNonQuery();
                }
            }
        }
        
        
        // Проверка юзера на наличие
        public static bool CheckUser(long user_id)
        {
            try
            {
                using var con = new NpgsqlConnection(user_db_connection);
                con.Open();
                var sql = $"SELECT * FROM users_table WHERE user_id = '{user_id.ToString()}'";
                using var cmd = new NpgsqlCommand(sql, con);
                var result = cmd.ExecuteScalar()!.ToString();
                con.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static void CreateUser(long userId, string username)
        {
            string state = "MainMenu";
            string wallet = "Не указан";
            int nft_number = 1;
            int total_price = 1; 
            string identifier = "1";
            
            using var con = new NpgsqlConnection(user_db_connection);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            cmd.CommandText = $"INSERT INTO users_table (user_id, username, state, wallet, nft_number, total_price, identifier) VALUES (@user_id, @username, @state, @wallet, @nft_number, @total_price, @identifier)";
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@state", state);
            cmd.Parameters.AddWithValue("@wallet", wallet);
            cmd.Parameters.AddWithValue("@nft_number", nft_number);
            cmd.Parameters.AddWithValue("@total_price", total_price);
            cmd.Parameters.AddWithValue("@identifier", identifier);

            cmd.ExecuteNonQuery();
        }

        // Получить все параметры
        public static List<string> GetAllInfo(long userId)
        {
            List<string> parameters= new List<string>();
            using var con = new NpgsqlConnection(user_db_connection);
            con.Open();
            var sql = $"SELECT wallet, nft_number, total_price, identifier FROM users_table WHERE user_id = '{userId.ToString()}'";
            using var cmd = new NpgsqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    parameters.Add(reader.GetString(0));
                    parameters.Add(reader.GetString(1));
                    parameters.Add(reader.GetString(2));
                    parameters.Add(reader.GetString(3));
                }
                con.Close();
                return parameters;
            }    
        }

        // Получить STATE юзера
        public static string state(long user_id)
        {
            try
            {
                using var con = new NpgsqlConnection(user_db_connection);
                con.Open();
                var sql = $"SELECT state FROM users_table WHERE user_id = '{user_id.ToString()}'";
                using var cmd = new NpgsqlCommand(sql, con);
                var result = cmd.ExecuteScalar();
                return result!.ToString()!;
            }
            catch(Exception)
            {
                return "MainMenu";
            }
        }


        // Обновить STATE
        public static void UpdateState(long userId, string state)
        {
            using (var conn = new NpgsqlConnection(user_db_connection))
            {
                
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE users_table SET state = @q WHERE user_id = @n", conn))
                {
                    command.Parameters.AddWithValue("n", userId.ToString());
                    command.Parameters.AddWithValue("q", state);
                    int nRows = command.ExecuteNonQuery();
                    
                }
            }
        }


        // Проверка кошелька
        public static bool CheckWallet(string wallet)
        {
            try
            {
                using var con = new NpgsqlConnection(user_db_connection);
                con.Open();
                var sql = $"SELECT * FROM users_table WHERE wallet = '{wallet}'";
                using var cmd = new NpgsqlCommand(sql, con);
                var result = cmd.ExecuteScalar()!.ToString();
                con.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Получить Wallet
        public static string GetWallet(long userId)
        {
            using var con = new NpgsqlConnection(user_db_connection);
            con.Open();
            var sql = $"SELECT wallet FROM users_table WHERE user_id = '{userId.ToString()}'";
            using var cmd = new NpgsqlCommand(sql, con);
            var result = cmd.ExecuteScalar();
            return result!.ToString()!;
        }

        // Обновить Wallet
        public static void UpdateWallet(long userId, string wallet)
        {
            using (var conn = new NpgsqlConnection(user_db_connection))
            {
                
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE users_table SET wallet = @q WHERE user_id = @n", conn))
                {
                    command.Parameters.AddWithValue("n", userId.ToString());
                    command.Parameters.AddWithValue("q", wallet);
                    int nRows = command.ExecuteNonQuery();
                    
                }
            }
        }

        // Получить Wallet
        public static string GetParsedNftCount(long userId)
        {
            try
            {
                using var con = new NpgsqlConnection(user_db_connection);
                con.Open();
                var sql = $"SELECT nft_count FROM parsed_users WHERE wallet_from = '{GetWallet(userId)}'";
                using var cmd = new NpgsqlCommand(sql, con);
                var result = cmd.ExecuteScalar();
                return result!.ToString()!;
            }
            catch(Exception){ return "0"; }
        }

        // Обновить nft_number
        public static void UpdateNftNumber(long userId, int nftNumber)
        {
            using (var conn = new NpgsqlConnection(user_db_connection))
            {
                
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE users_table SET nft_number = @q WHERE user_id = @n", conn))
                {
                    command.Parameters.AddWithValue("n", userId.ToString());
                    command.Parameters.AddWithValue("q", nftNumber.ToString());
                    int nRows = command.ExecuteNonQuery();
                }
            }
        }

        // Обновить total_price
        public static void UpdateTotalPrice(long userId, int totalPrice)
        {
            using (var conn = new NpgsqlConnection(user_db_connection))
            {
                
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE users_table SET total_price = @q WHERE user_id = @n", conn))
                {
                    command.Parameters.AddWithValue("n", userId.ToString());
                    command.Parameters.AddWithValue("q", totalPrice.ToString());
                    int nRows = command.ExecuteNonQuery();
                }
            }
        }

        // Обновить identifier
        public static void UpdateIdentifier(long userId, string identifier)
        {
            using (var conn = new NpgsqlConnection(user_db_connection))
            {
                
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE users_table SET identifier = @q WHERE user_id = @n", conn))
                {
                    command.Parameters.AddWithValue("n", userId.ToString());
                    command.Parameters.AddWithValue("q", identifier);
                    int nRows = command.ExecuteNonQuery();
                }
            }
        }
    }
}