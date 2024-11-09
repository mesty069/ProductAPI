namespace Product.API.Errors
{
    public class BaseCommonResponse
    {
        public BaseCommonResponse(int stuatusCode, string message = null)
        {
            StuatusCode = stuatusCode;
            Message = message ?? DefaultMessageForSatusCode(stuatusCode);
        }

        /// <summary>
        /// 根據 HTTP 狀態碼返回預設的錯誤訊息。
        /// </summary>
        /// <param name="stuatusCode"></param>
        /// <returns></returns>
        private string DefaultMessageForSatusCode(int stuatusCode)
         => stuatusCode switch
         {
             400 => "錯誤的請求",
             401 => "未授權",
             404 => "資源未找到",
             500 => "伺服器錯誤",
             _ => null
         };


        public int StuatusCode { get; set; }
        public string Message { get; set; }
    }
}
