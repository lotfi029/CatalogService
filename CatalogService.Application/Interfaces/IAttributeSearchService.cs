using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Interfaces;

public interface IAttributeSearchService : IElasticsearchService<AttributeDetailedResponse>;