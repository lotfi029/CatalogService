# Catalog Service

A robust, scalable e-commerce catalog microservice built with .NET 10, implementing Domain-Driven Design (DDD) and Clean Architecture principles.

## 🏗️ Architecture

This project follows Clean Architecture with clear separation of concerns:

```
CatalogService/
├── CatalogService.API/           # Presentation Layer (Minimal APIs, Endpoints)
├── CatalogService.Application/   # Application Layer (CQRS, DTOs, Validators)
├── CatalogService.Domain/        # Domain Layer (Entities, Value Objects, Domain Services)
├── CatalogService.Infrastructure/# Infrastructure Layer (EF Core, Repositories)
└── SharedKernel/                 # Shared abstractions (Result, Error, Entity)
```

### Design Patterns & Principles

- **Domain-Driven Design (DDD)**: Rich domain models with business logic encapsulation
- **CQRS**: Command Query Responsibility Segregation for scalable read/write operations
- **Repository Pattern**: Abstraction layer for data access
- **Unit of Work**: Transaction management across repositories
- **Result Pattern**: Explicit error handling without exceptions
- **Domain Events**: Decoupled communication between aggregates
- **Specification Pattern**: Reusable query logic

## 🚀 Features

### Core Domain Entities

#### Products
- Product lifecycle management (Draft → Active → Inactive → Archive)
- Multi-vendor support
- Flexible attribute system
- Product variants with SKU generation
- Category associations with primary category support

#### Categories
- Hierarchical category tree structure
- Slug-based URLs for SEO
- Variant attribute definitions per category
- Category moving with automatic path recalculation
- Soft delete with restore capability

#### Attributes
- Flexible attribute types (Select, Text, Boolean)
- Filterable and searchable attributes
- Dynamic option values
- Attribute activation/deactivation

#### Variants
- Product variants with customizable options
- Inventory-affecting vs cosmetic variants
- Pricing per variant (with compare-at pricing)
- Multi-currency support
- Custom variant options

### Technical Features

- **API Versioning**: Built-in versioning support with URL segment routing
- **Validation**: FluentValidation for comprehensive input validation
- **Error Handling**: Global exception handler with ProblemDetails
- **Health Checks**: Endpoint monitoring readiness
- **Swagger/Scalar**: Interactive API documentation
- **Audit Trail**: Automatic tracking of creation, updates, and deletions
- **Soft Delete**: Recoverable data deletion with query filters

## 🛠️ Technology Stack

- **.NET 10**: Latest framework features and performance improvements
- **ASP.NET Core Minimal APIs**: Lightweight, fast endpoint routing
- **Entity Framework Core 10**: Modern ORM with advanced querying
- **PostgreSQL**: Primary data store
- **FluentValidation**: Declarative validation rules
- **Mapster**: High-performance object mapping
- **Scrutor**: Assembly scanning for DI registration
- **Dapper**: Lightweight ORM for query operations

## 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/) (optional, for containerization)

## 🔧 Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/catalog-service.git
cd catalog-service
```

### 2. Configure Database Connection

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "host=localhost;port=5432;database=ecommerce_catalogservice_v01;username=postgres;password=YourPassword"
  }
}
```

### 3. Run Migrations

```bash
cd CatalogService.API
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5246`
- HTTPS: `https://localhost:7041`
- Swagger UI: `https://localhost:7041/swagger`
- Health Check: `https://localhost:7041/health`

## 🐳 Docker Deployment

### Build Image

```bash
docker build -t catalog-service:latest -f CatalogService.API/Dockerfile .
```

### Run Container

```bash
docker run -d \
  -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="host=postgres;port=5432;database=catalog_db;username=postgres;password=postgres" \
  --name catalog-service \
  catalog-service:latest
```

## 📡 API Endpoints

### Categories

```http
POST   /api/v1/categories                          # Create category
GET    /api/v1/categories/{id}                     # Get by ID
GET    /api/v1/categories/slug/{slug}              # Get by slug
GET    /api/v1/categories/tree?parentId={id}       # Get category tree
POST   /api/v1/categories/{id}/update-details      # Update details
PUT    /api/v1/categories/{id}/move?newParent={id} # Move category
DELETE /api/v1/categories/{id}                     # Delete category
```

### Products

```http
POST   /api/v1/products              # Create product
POST   /api/v1/products/bulk         # Bulk create
GET    /api/v1/products              # Get all products
GET    /api/v1/products/{id}         # Get by ID
PUT    /api/v1/products/{id}         # Update details
PATCH  /api/v1/products/{id}/active  # Activate product
PATCH  /api/v1/products/{id}/archive # Archive product
```

### Attributes

```http
POST   /api/v1/attributes                    # Create attribute
GET    /api/v1/attributes                    # Get all
GET    /api/v1/attributes/{id}               # Get by ID
GET    /api/v1/attributes/code/{code}        # Get by code
GET    /api/v1/attributes/type/{type}        # Get by type
PUT    /api/v1/attributes/{id}/details       # Update details
PUT    /api/v1/attributes/{id}/options       # Update options
PATCH  /api/v1/attributes/{id}/activate      # Activate
PATCH  /api/v1/attributes/{id}/deactivate    # Deactivate
DELETE /api/v1/attributes/{id}               # Delete
```

### Variant Attributes

```http
POST   /api/v1/variant-attributes              # Create variant attribute
POST   /api/v1/variant-attributes/bulk         # Bulk create
GET    /api/v1/variant-attributes              # Get all
GET    /api/v1/variant-attributes/{id}         # Get by ID
PUT    /api/v1/variant-attributes/{id}         # Update
DELETE /api/v1/variant-attributes/{id}         # Delete
```

### Product Categories

```http
POST   /api/v1/products/{productId}/categories/{categoryId}  # Add category
PATCH  /api/v1/products/{productId}/categories/{categoryId}  # Update
GET    /api/v1/products/{productId}/categories/{categoryId}  # Get
GET    /api/v1/products/{productId}/categories               # Get all
```

### Product Variants

```http
PUT    /api/v1/product-variants/{id}/custom-options  # Update custom options
PUT    /api/v1/product-variants/{id}/price           # Update price
DELETE /api/v1/product-variants/{id}                 # Delete variant
DELETE /api/v1/product-variants/product/{productId}  # Delete all by product
GET    /api/v1/product-variants/{id}                 # Get by ID
GET    /api/v1/product-variants/by-product/{id}      # Get by product
GET    /api/v1/product-variants/sku?sku={sku}        # Get by SKU
```

### Product Attributes

```http
POST   /api/v1/products/{productId}/attributes/{attributeId}      # Add attribute
POST   /api/v1/products/{productId}/attributes/bulk               # Bulk add
PUT    /api/v1/products/{productId}/attributes/{attributeId}      # Update
DELETE /api/v1/products/{productId}/attributes/{attributeId}      # Delete
DELETE /api/v1/products/{productId}/attributes/{attributeId}/all  # Delete all
GET    /api/v1/products/{productId}/attributes                    # Get all
GET    /api/v1/products/{productId}/attributes/{attributeId}      # Get by ID
```

## 🧪 Testing

```bash
# Run unit tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover
```

## 📊 Database Schema Highlights

### Key Tables

- **Categories**: Hierarchical structure with materialized path
- **Products**: Core product information with status management
- **ProductVariants**: SKU-based variants with pricing
- **ProductAttributes**: Dynamic attributes per product
- **ProductCategories**: Many-to-many with primary flag
- **Attributes**: Reusable attribute definitions
- **VariantAttributeDefinition**: Template for variant options
- **CategoryVariantAttribute**: Category-specific variant requirements

### Relationships

- Products → Categories: Many-to-Many (with primary category)
- Products → Variants: One-to-Many
- Products → Attributes: Many-to-Many (with values)
- Categories → Variants: Many-to-Many (required/optional)
- Categories → Categories: Self-referencing hierarchy

## 🔐 Security Considerations

- Input validation on all endpoints via FluentValidation
- SQL injection prevention through parameterized queries
- Soft delete prevents accidental data loss
- Audit trails for compliance and debugging

## 🎯 Roadmap & Future Enhancements

### Phase 1: Search & Performance (Q1 2024)
- [ ] **Elasticsearch Integration**
  - Full-text search across products and categories
  - Faceted search with filters
  - Autocomplete suggestions
  - Search analytics

- [ ] **Caching Strategy**
  - Redis for distributed caching
  - Response caching for read-heavy operations
  - Cache invalidation on updates

### Phase 2: DevOps & Reliability (Q2 2024)
- [ ] **Monitoring & Observability**
  - OpenTelemetry integration
  - Distributed tracing with Jaeger
  - Metrics with Prometheus
  - Log aggregation with ELK Stack
  - Application Performance Monitoring (APM)
  - Custom dashboards in Grafana

- [ ] **Kubernetes Deployment**
  - Helm charts for easy deployment
  - Horizontal Pod Autoscaling (HPA)
  - Resource limits and requests
  - Liveness and readiness probes
  - ConfigMaps and Secrets management
  - Service mesh with Istio
  - Blue-Green deployments

- [ ] **CI/CD Pipeline**
  - GitHub Actions workflows
  - Automated testing on PR
  - Container scanning for vulnerabilities
  - Automated rollbacks
  - Staging and production environments

### Phase 3: Advanced Features (Q3 2024)
- [ ] **Event Sourcing**
  - Event store for complete audit history
  - Replay capabilities for debugging
  - CQRS with separate read models

- [ ] **GraphQL API**
  - HotChocolate integration
  - Flexible querying for clients
  - Batch operations support

- [ ] **Multi-tenancy**
  - Tenant isolation strategies
  - Tenant-specific configurations
  - Per-tenant analytics

### Phase 4: Scale & Performance (Q4 2024)
- [ ] **Database Optimization**
  - Read replicas for query scaling
  - Connection pooling optimization
  - Query performance tuning
  - Database sharding strategy

- [ ] **Message Queue Integration**
  - RabbitMQ/Kafka for async processing
  - Event-driven architecture
  - Saga pattern for distributed transactions

- [ ] **API Gateway**
  - Rate limiting
  - API versioning strategy
  - Request transformation
  - Authentication/Authorization

### Phase 5: Business Features
- [ ] **Inventory Management**
  - Stock tracking per variant
  - Low stock alerts
  - Reservation system

- [ ] **Price Management**
  - Dynamic pricing rules
  - Promotional pricing
  - Price history tracking
  - Bulk price updates

- [ ] **Media Management**
  - Image upload and optimization
  - CDN integration
  - Video support
  - 360° product views

- [ ] **Internationalization**
  - Multi-language support
  - Localized content
  - Regional pricing
  - Currency conversion

### Infrastructure Targets
- **Performance**: < 100ms p95 latency for read operations
- **Availability**: 99.9% uptime SLA
- **Scalability**: Support for 10K+ requests/second
- **Data**: Handle 1M+ products efficiently

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Coding Standards

- Follow C# coding conventions
- Write clean, self-documenting code
- Add XML comments for public APIs
- Ensure all tests pass
- Maintain test coverage above 80%

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Author

- mohamed lotfi - Initial work - [@github](https://github.com/lotfi029) - [@linkedin](https://www.linkedin.com/in/mohamedlotf/)

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin.
- Domain-Driven Design by Eric Evans
- CQRS pattern by Greg Young
- The .NET Community

## 📞 Support

For support, email mohamed.lotfi.dev@gmail.com or open an issue in the repository.

---

**Built with ❤️ using .NET 10**
