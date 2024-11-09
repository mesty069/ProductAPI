using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repository
{
    public class EncryptionService
    {
        private readonly AesEncryptionHelper _aesHelper;
        private readonly RsaEncryptionHelper _rsaHelper;

        public EncryptionService(AesEncryptionHelper aesHelper, RsaEncryptionHelper rsaHelper)
        {
            _aesHelper = aesHelper;
            _rsaHelper = rsaHelper;
        }

        public string EncryptData(string data)
        {
            return _aesHelper.Encrypt(data);
        }

        public string DecryptData(string encryptedData)
        {
            return _aesHelper.Decrypt(encryptedData);
        }
    }
}
