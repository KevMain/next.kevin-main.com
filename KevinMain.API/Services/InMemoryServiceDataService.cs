using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// In-memory implementation of Service data service.
/// Defines service offerings based on Kevin's skills and experience.
/// Can be replaced with a database-backed implementation by creating a new class
/// that implements IServiceDataService and swapping it in Program.cs.
/// </summary>
public class InMemoryServiceDataService : IServiceDataService
{
    public Task<ServiceData> GetServiceDataAsync()
    {
        var serviceData = new ServiceData
        {
            Categories = new List<ServiceCategory>
            {
                new ServiceCategory
                {
                    CategoryName = "Technical Leadership & Consulting",
                    CategoryDescription = "Strategic technical guidance and team leadership to drive engineering excellence and business value",
                    Icon = "🎯",
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Name = "Technical Leadership",
                            Description = "Provide technical direction and leadership for your engineering teams, ensuring best practices, architectural coherence, and continuous improvement.",
                            KeyFeatures = new List<string>
                            {
                                "Team mentoring and coaching",
                                "Technical strategy and roadmap development",
                                "Code review and quality assurance processes",
                                "Engineering standards and best practices",
                                "Stakeholder communication and alignment"
                            },
                            Technologies = new List<string> { ".NET", "Azure", "Agile", "DevOps" },
                            Deliverables = new List<string>
                            {
                                "Technical strategy document",
                                "Team capability assessment",
                                "Engineering standards playbook",
                                "Architecture decision records"
                            },
                            IdealFor = "Organizations needing temporary technical leadership or seeking to uplevel their engineering practices"
                        },
                        new Service
                        {
                            Name = "Architecture Consulting",
                            Description = "Design and review software architectures that balance business needs, technical excellence, and long-term maintainability.",
                            KeyFeatures = new List<string>
                            {
                                "System architecture design and review",
                                "Microservices and API-first architecture",
                                "Cloud migration strategy",
                                "Legacy system modernization planning",
                                "Performance and scalability optimization"
                            },
                            Technologies = new List<string> { "Azure", ".NET", "Microservices", "REST APIs", "Event-Driven Architecture" },
                            Deliverables = new List<string>
                            {
                                "Architecture diagrams and documentation",
                                "Technical design documents",
                                "Migration roadmap",
                                "Risk assessment and mitigation plan"
                            },
                            IdealFor = "Companies planning major technical initiatives, migrations, or facing architectural challenges"
                        }
                    }
                },
                new ServiceCategory
                {
                    CategoryName = "Cloud & Backend Development",
                    CategoryDescription = "Scalable, secure, and maintainable backend solutions powered by .NET and Azure",
                    Icon = "☁️",
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Name = "Azure Cloud Solutions",
                            Description = "Design, build, and deploy cloud-native applications on Microsoft Azure with best practices for security, scalability, and cost-effectiveness.",
                            KeyFeatures = new List<string>
                            {
                                "Azure App Services, Functions, and Container Apps",
                                "Azure SQL Database, Cosmos DB, and storage solutions",
                                "Azure DevOps CI/CD pipelines",
                                "Infrastructure as Code (ARM, Bicep)",
                                "Monitoring and diagnostics with Application Insights"
                            },
                            Technologies = new List<string> { "Azure", "Azure DevOps", "App Services", "Functions", "SQL Database", "Cosmos DB" },
                            Deliverables = new List<string>
                            {
                                "Deployed cloud infrastructure",
                                "Automated CI/CD pipelines",
                                "Monitoring and alerting setup",
                                "Documentation and runbooks"
                            },
                            IdealFor = "Businesses moving to the cloud or optimizing their existing Azure infrastructure"
                        },
                        new Service
                        {
                            Name = ".NET API Development",
                            Description = "Build robust, secure, and high-performance REST APIs using ASP.NET Core and modern .NET practices.",
                            KeyFeatures = new List<string>
                            {
                                "RESTful API design and implementation",
                                "Authentication and authorization (OAuth, JWT)",
                                "Entity Framework Core for data access",
                                "API versioning and documentation",
                                "Performance optimization and caching"
                            },
                            Technologies = new List<string> { "ASP.NET Core", "C#", ".NET", "Entity Framework", "Dapper", "SQL Server" },
                            Deliverables = new List<string>
                            {
                                "Production-ready APIs",
                                "OpenAPI/Swagger documentation",
                                "Unit and integration tests",
                                "Deployment configuration"
                            },
                            IdealFor = "Companies needing backend services for web, mobile, or third-party integrations"
                        },
                        new Service
                        {
                            Name = "Microservices Development",
                            Description = "Transform monolithic applications into independently deployable microservices for improved scalability and team autonomy.",
                            KeyFeatures = new List<string>
                            {
                                "Domain-driven design and bounded contexts",
                                "Service-to-service communication patterns",
                                "Event-driven architectures",
                                "API gateway and service mesh",
                                "Containerization with Docker"
                            },
                            Technologies = new List<string> { ".NET", "Docker", "Azure", "REST APIs", "Message Queues", "Event-Driven" },
                            Deliverables = new List<string>
                            {
                                "Decomposed microservices",
                                "Service architecture diagrams",
                                "Communication contracts",
                                "Deployment pipelines"
                            },
                            IdealFor = "Organizations looking to modernize monolithic applications or build new scalable systems"
                        }
                    }
                },
                new ServiceCategory
                {
                    CategoryName = "AI & Innovation",
                    CategoryDescription = "Leverage artificial intelligence to enhance products and development workflows",
                    Icon = "🤖",
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Name = "AI Integration & Azure OpenAI",
                            Description = "Integrate Azure OpenAI services into your applications to add intelligent features like natural language processing, content generation, and smart assistants.",
                            KeyFeatures = new List<string>
                            {
                                "Azure OpenAI service integration",
                                "LLM-powered features (chat, summarization, analysis)",
                                "Prompt engineering and optimization",
                                "Custom AI workflows and automations",
                                "RAG (Retrieval Augmented Generation) implementations"
                            },
                            Technologies = new List<string> { "Azure OpenAI", "GPT-4", "Semantic Kernel", ".NET", "Python" },
                            Deliverables = new List<string>
                            {
                                "AI-powered features",
                                "Prompt templates and guidelines",
                                "Integration documentation",
                                "Cost optimization recommendations"
                            },
                            IdealFor = "Businesses wanting to add AI capabilities to their products or automate knowledge work"
                        },
                        new Service
                        {
                            Name = "AI-Assisted Development Setup",
                            Description = "Implement AI coding assistants like GitHub Copilot to boost developer productivity across your engineering organization.",
                            KeyFeatures = new List<string>
                            {
                                "GitHub Copilot implementation and training",
                                "AI tooling strategy and governance",
                                "Developer productivity measurement",
                                "Best practices and prompt engineering for code generation",
                                "Team workshops and adoption support"
                            },
                            Technologies = new List<string> { "GitHub Copilot", "AI Code Assistants", "VS Code", "Visual Studio" },
                            Deliverables = new List<string>
                            {
                                "Configured AI tooling",
                                "Training materials and workshops",
                                "Best practices documentation",
                                "Productivity metrics dashboard"
                            },
                            IdealFor = "Development teams looking to accelerate delivery with AI-powered coding tools"
                        }
                    }
                },
                new ServiceCategory
                {
                    CategoryName = "Frontend Development",
                    CategoryDescription = "Modern, responsive web applications using React and TypeScript",
                    Icon = "🎨",
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Name = "React Application Development",
                            Description = "Build modern, responsive single-page applications using React and TypeScript with clean architecture and excellent user experience.",
                            KeyFeatures = new List<string>
                            {
                                "React with TypeScript development",
                                "State management (Redux, Context API)",
                                "Component library integration (Material-UI, Ant Design)",
                                "Responsive design and mobile-first approach",
                                "API integration and data fetching"
                            },
                            Technologies = new List<string> { "React", "TypeScript", "Redux", "Material-UI", "Vite", "Webpack" },
                            Deliverables = new List<string>
                            {
                                "Production-ready React application",
                                "Reusable component library",
                                "Responsive UI/UX",
                                "Integration with backend APIs"
                            },
                            IdealFor = "Businesses needing modern web applications with rich user experiences"
                        },
                        new Service
                        {
                            Name = "Legacy Frontend Modernization",
                            Description = "Migrate legacy frontend applications (AngularJS, jQuery, etc.) to modern frameworks like React while maintaining business continuity.",
                            KeyFeatures = new List<string>
                            {
                                "Migration strategy and planning",
                                "Incremental migration approach",
                                "Code modernization and refactoring",
                                "Testing and quality assurance",
                                "Knowledge transfer and training"
                            },
                            Technologies = new List<string> { "React", "Angular", "TypeScript", "Webpack", "Module Federation" },
                            Deliverables = new List<string>
                            {
                                "Migration roadmap",
                                "Modernized frontend application",
                                "Updated documentation",
                                "Team training sessions"
                            },
                            IdealFor = "Companies with aging frontend technologies that need modernization without disrupting users"
                        }
                    }
                },
                new ServiceCategory
                {
                    CategoryName = "Legacy System Modernization",
                    CategoryDescription = "Transform legacy applications into modern, maintainable systems",
                    Icon = "🔄",
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Name = "Legacy Application Modernization",
                            Description = "Modernize legacy systems (Classic ASP, Web Forms, etc.) to current technologies while preserving business logic and ensuring zero downtime.",
                            KeyFeatures = new List<string>
                            {
                                "Legacy code assessment and analysis",
                                "Incremental migration strategy",
                                "Strangler fig pattern implementation",
                                "Database modernization",
                                "Risk mitigation and rollback plans"
                            },
                            Technologies = new List<string> { ".NET", "ASP.NET Core", "Azure", "SQL Server", "Entity Framework" },
                            Deliverables = new List<string>
                            {
                                "Modernization roadmap",
                                "Migrated application components",
                                "Updated architecture documentation",
                                "Deployment and rollback procedures"
                            },
                            IdealFor = "Organizations with mission-critical legacy applications requiring modernization"
                        }
                    }
                },
                new ServiceCategory
                {
                    CategoryName = "DevOps & Quality Engineering",
                    CategoryDescription = "Improve delivery speed and quality through automation and best practices",
                    Icon = "⚙️",
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Name = "CI/CD Pipeline Development",
                            Description = "Design and implement automated CI/CD pipelines to accelerate delivery and reduce deployment risks.",
                            KeyFeatures = new List<string>
                            {
                                "Azure DevOps or GitHub Actions pipelines",
                                "Automated build, test, and deployment",
                                "Environment management (dev, staging, prod)",
                                "Feature flags and progressive rollouts",
                                "Deployment monitoring and rollback automation"
                            },
                            Technologies = new List<string> { "Azure DevOps", "GitHub Actions", "Docker", "Azure", "PowerShell" },
                            Deliverables = new List<string>
                            {
                                "Automated CI/CD pipelines",
                                "Deployment documentation",
                                "Release management process",
                                "Monitoring and alerting configuration"
                            },
                            IdealFor = "Teams wanting to increase deployment frequency and reduce manual deployment errors"
                        },
                        new Service
                        {
                            Name = "Testing Strategy & Implementation",
                            Description = "Establish comprehensive testing practices including unit, integration, and automated testing to improve code quality.",
                            KeyFeatures = new List<string>
                            {
                                "Test strategy development",
                                "Unit testing with xUnit/NUnit",
                                "Integration testing",
                                "Test automation frameworks",
                                "Code coverage analysis"
                            },
                            Technologies = new List<string> { "xUnit", "NUnit", "Moq", "SpecFlow", "Playwright", "Jest" },
                            Deliverables = new List<string>
                            {
                                "Testing strategy document",
                                "Comprehensive test suites",
                                "Test automation framework",
                                "Team training on testing practices"
                            },
                            IdealFor = "Development teams looking to reduce defects and improve software quality"
                        }
                    }
                }
            }
        };

        return Task.FromResult(serviceData);
    }
}
