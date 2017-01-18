using Nethereum.Hex.HexTypes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Nethereum.Web3.Tests
{
    public class QuoromBasicTests
    {
        private readonly ITestOutputHelper output;
        public QuoromBasicTests(ITestOutputHelper output)
        {
            this.output = output;
            
        }

        [Fact( DisplayName ="Quorom Basic Tests")]
        public async void SendAContract()
        {
            output.WriteLine("starting");
           
            var senderAddress = "0xed9d02e382b34818e88b88a309c7fe71e65f419d";
            var web3 = new Web3("http://40.115.79.225:22000");
            //var web3 = new Web3("http://localhost:22003/");
            var password = "";

            var abi = @"[{""constant"":true,""inputs"":[],""name"":""storedData"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[{""name"":""x"",""type"":""uint256""}],""name"":""set"",""outputs"":[],""payable"":false,""type"":""function""},{""constant"":true,""inputs"":[],""name"":""get"",""outputs"":[{""name"":""retVal"",""type"":""uint256""}],""payable"":false,""type"":""function""},{""inputs"":[{""name"":""initVal"",""type"":""uint256""}],""type"":""constructor""}]";

            var byteCode = "0x6060604052604051602080608983395060806040525160008190555060628060276000396000f3606060405260e060020a60003504632a1afcd98114603057806360fe47b114603c5780636d4ce63c146048575b6002565b34600257605060005481565b34600257600435600055005b346002576000545b60408051918252519081900360200190f3";

            var multiplier = 10;
            string[] privateFor = { "ROAZBWtSacxXQrOe3FGAqJDyJjFePR5ce4TSIzmJ0Bc=" };// { "vM14B2THvpjuVXMM6zxrnsRmQgSRtkhqtTEG2iOA+WI=" };
            var transactionHash = await web3.Eth.DeployContract.SendRequestAsync(
                abi,
                byteCode,
                senderAddress,
                new HexBigInteger(600000),
                privateFor,
                multiplier);


            // var miningResult = await web3.Miner.Start.SendRequestAsync(6);
            // Assert.True(miningResult);

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }
            var contractAddress = receipt.ContractAddress;
            output.WriteLine(contractAddress);

            var contract = web3.Eth.GetContract(abi, contractAddress);

            var multiplyFunction = contract.GetFunction("get");

            var result = await multiplyFunction.CallAsync<int>();

            output.WriteLine(result.ToString());
            //Assert.Equal(multiplier, result);
        }



    }
}
