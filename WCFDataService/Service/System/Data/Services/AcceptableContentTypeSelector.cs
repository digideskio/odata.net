//   OData .NET Libraries ver. 5.6.2
//   Copyright (c) Microsoft Corporation. All rights reserved.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

namespace System.Data.Services
{
    /// <summary>
    /// Strategy for getting the content type based on $format and the accept header.
    /// </summary>
    internal interface IAcceptableContentTypeSelector
    {
        /// <summary>
        /// Gets a comma-separated list of client-supported MIME Accept types.
        /// If $format is found with a value we recognize, we must use the matching content type and ignore accept header.
        /// If $format is found but we don't recognize the value, return just that value (don't even concat to accept values). Later we'll probably throw.
        /// If no $format, use accept header value (might be a list).  Returning null indicates neither was set.
        /// </summary>
        /// <param name="dollarFormatValue">Value of $format or null.</param>
        /// <param name="acceptHeaderValue">Value of accept header or null.</param>
        /// <param name="maxDataServiceVersion">MaxDataServiceVersion as specified by the request or determined from our logic.</param>
        /// <returns>A comma-separated list of client-supported MIME Accept types.</returns>
        string GetFormat(string dollarFormatValue, string acceptHeaderValue, Version maxDataServiceVersion);
    }

    /// <summary>
    /// Strategy for getting the content type based on $format and the accept header for V3.
    /// </summary>
    internal class AcceptableContentTypeSelector : IAcceptableContentTypeSelector
    {
        /// <summary>
        /// Gets a comma-separated list of client-supported MIME Accept types.
        /// If $format is found with a value we recognize, we must use the matching content type and ignore accept header.
        /// If $format is found but we don't recognize the value, return just that value (don't even concat to accept values). Later we'll probably throw.
        /// If no $format, use accept header value (might be a list).  Returning null indicates neither was set.
        /// </summary>
        /// <param name="dollarFormatValue">Value of $format or null.</param>
        /// <param name="acceptHeaderValue">Value of accept header or null.</param>
        /// <param name="maxDataServiceVersion">MaxDataServiceVersion as specified by the request or determined from our logic.</param>
        /// <returns>A comma-separated list of client-supported MIME Accept types.</returns>
        public string GetFormat(string dollarFormatValue, string acceptHeaderValue, Version maxDataServiceVersion)
        {
            if (dollarFormatValue != null)
            {
                if (maxDataServiceVersion < new Version(3, 0))
                {
                    switch (dollarFormatValue)
                    {
                        case "xml":
                            return XmlConstants.MimeApplicationXml;
                        case "atom":
                            return XmlConstants.MimeApplicationAtom;
                        case "json":
                            return XmlConstants.MimeApplicationJson;
                        default:
                            return dollarFormatValue;
                    }
                }

                // V3+
                switch (dollarFormatValue)
                {
                    case "xml":
                        return XmlConstants.MimeApplicationXml;
                    case "atom":
                        return XmlConstants.MimeApplicationAtom;
                    case "json":
                        return XmlConstants.MimeApplicationJson;
                    case "verbosejson":
                        return XmlConstants.MimeApplicationJsonODataVerbose;
                    default:
                        return dollarFormatValue;
                }
            }

            return acceptHeaderValue;
        }
    }
}