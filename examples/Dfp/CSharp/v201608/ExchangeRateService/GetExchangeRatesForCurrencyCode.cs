// Copyright 2016, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201608;
using Google.Api.Ads.Dfp.v201608;

using System;
namespace Google.Api.Ads.Dfp.Examples.CSharp.v201608 {
  /// <summary>
  /// This example gets the exchange rate for a specific currency code.
  /// </summary>
  public class GetExchangeRatesForCurrencyCode : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets the exchange rate for a specific currency code.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetExchangeRatesForCurrencyCode codeExample = new GetExchangeRatesForCurrencyCode();
      Console.WriteLine(codeExample.Description);

      string currencyCode = _T("INSERT_CURRENCY_CODE_HERE");
      codeExample.Run(new DfpUser(), currencyCode);
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser user, string currencyCode) {
      ExchangeRateService exchangeRateService =
          (ExchangeRateService) user.GetService(DfpService.v201608.ExchangeRateService);

      // Create a statement to select exchange rates.
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("currencyCode = :currencyCode")
          .OrderBy("id ASC")
          .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
          .AddValue("currencyCode", currencyCode);

      // Retrieve a small amount of exchange rates at a time, paging through
      // until all exchange rates have been retrieved.
      ExchangeRatePage page = new ExchangeRatePage();
      try {
        do {
          page = exchangeRateService.getExchangeRatesByStatement(statementBuilder.ToStatement());

          if (page.results != null) {
            // Print out some information for each exchange rate.
            int i = page.startIndex;
            foreach (ExchangeRate exchangeRate in page.results) {
              Console.WriteLine("{0}) Exchange rate with ID \"{1}\", currency code \"{2}\", "
                  + "and exchange rate \"{3}\" was found.",
                  i++,
                  exchangeRate.id,
                  exchangeRate.currencyCode,
                  exchangeRate.exchangeRate / 10000000000);
            }
          }

          statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
        } while (statementBuilder.GetOffset() < page.totalResultSetSize);

        Console.WriteLine("Number of results found: {0}", page.totalResultSetSize);
      } catch (Exception e) {
        Console.WriteLine("Failed to get exchange rates. Exception says \"{0}\"",
            e.Message);
      }
    }
  }
}
