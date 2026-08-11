const _mockPosts = [
  {
    id: 'ai-for-dotnet-developers-copilot-is-more-than-better-autocomplete',
    title: 'AI for .NET Developers: Copilot Is More Than Better Autocomplete',
    slug: 'ai-for-dotnet-developers-copilot-is-more-than-better-autocomplete',
    contentPreview: "I've been writing C# and .NET professionally for a long time, so when I first started using AI tools such as GitHub Copilot, I wasn't entirely sure what they were going to add.",
    content: `I've been writing C# and .NET professionally for a long time, so when I first started using AI tools such as GitHub Copilot, I wasn't entirely sure what they were going to add.

I don't really need help writing a foreach loop or creating another ASP.NET Core controller.

And if all Copilot did was save me a few keystrokes, it would be useful — but not particularly interesting.

What I've found more valuable is using AI for everything around writing the code.

## Stop Thinking About Code Generation

Code generation is the obvious place to start with Copilot. It can quickly produce DTOs, mapping code, straightforward methods and unit tests.

But increasingly, I'm using it to:

- understand unfamiliar code
- identify edge cases
- challenge implementation decisions
- investigate bugs
- review code and tests

I'm starting to think of Copilot less as autocomplete and more as another developer I can bounce ideas off.

Admittedly, it's a developer whose pull requests I'd review very carefully.

## Ask Copilot to Challenge You

This has probably been the biggest change in how I use AI.

Rather than:

> Improve this code.

I'll ask something more specific:

> Review this ASP.NET Core endpoint. Look for correctness, security, unnecessary database calls, error handling and missing cancellation support. Explain the problems before suggesting changes.

I'll also use it to challenge decisions I've already made:

> I don't think this functionality needs to become a microservice. Give me the strongest argument against that decision.

I find that much more useful than asking AI to make the architectural decision for me.

I make the decision. AI helps me question it.

## AI Is Great for Finding Missing Tests

Testing is another area where Copilot works particularly well with .NET.

Rather than asking it to immediately generate tests for some C# code, try:

> List the behaviours, boundary conditions and edge cases that should be tested. Don't write the tests yet.

You can review those scenarios first, decide which ones actually matter and then ask Copilot to generate the repetitive parts.

For anyone already using TDD, this fits quite naturally. You're still defining the behaviour; AI is helping you explore what you might have missed.

## Context Makes a Huge Difference

Vague questions produce vague answers.

Instead of:

> Write a customer service.

Try:

> Implement CustomerService using the same patterns as OrderService. Use our existing EF Core DbContext, propagate CancellationToken and don't introduce another abstraction.

The more Copilot understands about your existing application and its constraints, the more useful its suggestions become.

That's especially important in mature .NET applications, where consistency with the existing architecture often matters more than producing the theoretically “perfect” solution.

## Plausible Doesn't Mean Correct

AI can produce extremely convincing C#.

It can also produce extremely convincing wrong C#.

Code can compile while still containing unnecessary database calls, concurrency problems, security issues or simply incorrect assumptions about how your application works.

That's why I treat AI-generated code like somebody else's pull request.

I need to understand it before I accept it.

And that's perhaps where experience becomes more important rather than less. Generating code is becoming cheap. Knowing whether that code actually belongs in your application isn't.

## It's Still Software Engineering

There's a lot of talk about developers needing to learn “prompt engineering”, but I'm not convinced it's anything particularly mysterious.

The skill that matters most is one experienced developers already use every day:

**being precise.**

Give Copilot the problem, the context and the constraints. Ask it to challenge assumptions rather than blindly produce a solution.

The more I use AI with .NET, the less interested I am in whether Copilot can write C# faster than I can.

Of course it can.

What's more useful is whether it can help me understand something faster, spot something I've missed or challenge a decision before it reaches production.

How are you using AI with .NET? Has Copilot genuinely changed how you work, or is it still mostly clever autocomplete?`,
    publishedAt: new Date().toISOString()
  }
];

export function getPosts() {
  return _mockPosts;
}

export function getPostBySlug(slug) {
  return _mockPosts.find(post => post.slug === slug) ?? null;
}
