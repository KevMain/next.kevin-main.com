const _mockPosts = [
  {
    id: crypto.randomUUID(),
    title: 'Getting Started with .NET 10',
    slug: 'getting-started-with-dotnet-10',
    contentPreview: 'A look at the newest features in .NET 10 and how to start building modern, high-performance applications with the latest tooling.',
    content: `.NET 10 brings a wealth of improvements across the runtime, libraries, and tooling. In this post we take a tour of the highlights and what they mean for your day-to-day development.

Performance continues to be a headline theme, with further JIT enhancements, improved garbage collection, and faster startup times out of the box. Combined with native AOT maturing across more workloads, .NET 10 applications are leaner and quicker than ever.

Getting started is as simple as installing the latest SDK, updating your target framework to net10.0, and rebuilding. Most projects upgrade with zero code changes, and the tooling in Visual Studio makes the process almost effortless.`,
    publishedAt: new Date().toISOString()
  },
  {
    id: crypto.randomUUID(),
    title: 'Building Resilient Cloud Architectures on Azure',
    slug: 'building-resilient-cloud-architectures-on-azure',
    contentPreview: 'Practical patterns for designing fault-tolerant, scalable systems in Azure, from retry policies to multi-region deployments.',
    content: `Resilience is not a feature you bolt on at the end — it has to be designed in from the start. In this post we look at practical patterns for building fault-tolerant systems on Azure.

Start with the basics: retry policies with exponential backoff, circuit breakers, and sensible timeouts. Libraries like Polly make these patterns straightforward to adopt in .NET applications, and Azure SDKs ship with solid defaults.

Beyond the code, architecture matters. Availability zones protect against datacenter failures, while multi-region deployments with Azure Front Door give you geographic redundancy. Pair these with health checks and automated failover, and your system can weather most storms without anyone noticing.`,
    publishedAt: new Date().toISOString()
  }
];

export function getPosts() {
  return _mockPosts;
}

export function getPostBySlug(slug) {
  return _mockPosts.find(post => post.slug === slug) ?? null;
}
