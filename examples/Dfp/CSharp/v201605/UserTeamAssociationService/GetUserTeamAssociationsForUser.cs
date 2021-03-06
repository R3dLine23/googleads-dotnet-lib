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
using Google.Api.Ads.Dfp.Util.v201605;
using Google.Api.Ads.Dfp.v201605;

using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201605 {
  /// <summary>
  /// This example gets all user team associations (i.e. teams) for a given user.
  /// </summary>
  public class GetUserTeamAssociationsForUser : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all user team associations (i.e. teams) for a given user.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetUserTeamAssociationsForUser codeExample = new GetUserTeamAssociationsForUser();
      Console.WriteLine(codeExample.Description);

      long userId = long.Parse(_T("INSERT_USER_ID_HERE"));
      codeExample.Run(new DfpUser(), userId);
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser user, long userId) {
      UserTeamAssociationService userTeamAssociationService =
          (UserTeamAssociationService) user.GetService(
          DfpService.v201605.UserTeamAssociationService);

      // Create a statement to select user team associations.
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("userId = :userId")
          .OrderBy("userId ASC, teamId ASC")
          .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
          .AddValue("userId", userId);

      // Retrieve a small amount of user team associations at a time, paging through
      // until all user team associations have been retrieved.
      UserTeamAssociationPage page = new UserTeamAssociationPage();
      try {
        do {
          page = userTeamAssociationService.getUserTeamAssociationsByStatement(
              statementBuilder.ToStatement());

          if (page.results != null) {
            // Print out some information for each user team association.
            int i = page.startIndex;
            foreach (UserTeamAssociation userTeamAssociation in page.results) {
              Console.WriteLine("{0}) User team association with user ID \"{1}\" "
                  + "and team ID \"{2}\" was found.",
                  i++,
                  userTeamAssociation.userId,
                  userTeamAssociation.teamId);
            }
          }

          statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
        } while (statementBuilder.GetOffset() < page.totalResultSetSize);

        Console.WriteLine("Number of results found: {0}", page.totalResultSetSize);
      } catch (Exception e) {
        Console.WriteLine("Failed to get user team associations. Exception says \"{0}\"",
            e.Message);
      }
    }
  }
}
