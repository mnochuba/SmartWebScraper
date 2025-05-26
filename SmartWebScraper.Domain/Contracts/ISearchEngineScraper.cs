using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWebScraper.Domain.Contracts;
public interface ISearchEngineScraper
{
    Task<Dictionary<int, string>> GetSearchResultPositionsAsync(string searchPhrase, string targetUrl);
}
