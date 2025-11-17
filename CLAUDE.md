# CLAUDE.md - AI Assistant Guide for ClaudeCode Repository

> **Last Updated:** 2025-11-17
> **Repository:** indujanT/ClaudeCode
> **Current Status:** Initial Setup / Minimal Codebase

---

## Table of Contents

1. [Repository Overview](#repository-overview)
2. [Codebase Structure](#codebase-structure)
3. [Development Workflows](#development-workflows)
4. [Key Conventions](#key-conventions)
5. [Git Workflow](#git-workflow)
6. [Testing Strategy](#testing-strategy)
7. [Code Patterns](#code-patterns)
8. [AI Assistant Guidelines](#ai-assistant-guidelines)
9. [Common Tasks](#common-tasks)
10. [Troubleshooting](#troubleshooting)

---

## Repository Overview

### Current State
This repository is in its **initial setup phase**. The codebase currently contains:
- A single `Test` file with minimal content
- Git repository initialized with one commit
- No build system or dependencies configured yet

### Project Purpose
*[TO BE DOCUMENTED: Describe the main purpose and goals of this project]*

### Technology Stack
*[TO BE DOCUMENTED: List the primary technologies, frameworks, and tools used]*

### Key Dependencies
*[TO BE DOCUMENTED: Document major dependencies and their versions]*

---

## Codebase Structure

### Current Directory Layout
```
ClaudeCode/
├── .git/           # Git repository metadata
└── Test            # Initial test file
```

### Expected Future Structure
*[TO BE DOCUMENTED: As the project grows, document the directory structure here]*

Example structure for future reference:
```
ClaudeCode/
├── src/            # Source code
├── tests/          # Test files
├── docs/           # Documentation
├── config/         # Configuration files
├── scripts/        # Build and utility scripts
├── .github/        # GitHub workflows and templates
├── package.json    # Node.js dependencies (if applicable)
├── README.md       # User-facing documentation
└── CLAUDE.md       # This file
```

---

## Development Workflows

### Setup Instructions

#### Initial Setup
*[TO BE DOCUMENTED: Steps for new developers to set up the project]*

Current setup (minimal):
```bash
git clone <repository-url>
cd ClaudeCode
# No additional setup required yet
```

#### Future Setup (Template)
```bash
# Clone the repository
git clone <repository-url>
cd ClaudeCode

# Install dependencies
# [Add appropriate commands: npm install, pip install -r requirements.txt, etc.]

# Run initial build
# [Add build commands if applicable]

# Verify setup
# [Add verification commands]
```

### Development Process

1. **Feature Development**
   - Create a feature branch from the main branch
   - Follow naming convention: `claude/<feature-name>-<session-id>`
   - Make incremental commits with clear messages
   - Test thoroughly before pushing
   - Create pull request for review

2. **Bug Fixes**
   - Create a bugfix branch
   - Include issue number in branch name if applicable
   - Add regression tests
   - Document the fix in commit messages

3. **Code Review**
   *[TO BE DOCUMENTED: Code review process and checklist]*

---

## Key Conventions

### File Organization
*[TO BE DOCUMENTED: How files should be organized and named]*

### Naming Conventions
*[TO BE DOCUMENTED: Conventions for variables, functions, classes, files, etc.]*

Example conventions to establish:
- **Variables:** camelCase for local variables, UPPER_CASE for constants
- **Functions:** descriptive verbs, camelCase
- **Files:** kebab-case for multi-word files
- **Classes:** PascalCase

### Code Style
*[TO BE DOCUMENTED: Style guide references and linting configuration]*

Future items to document:
- Indentation (spaces vs tabs, width)
- Line length limits
- Comment style
- Import organization
- Error handling patterns

---

## Git Workflow

### Branch Strategy

**Current Active Branch:** `claude/claude-md-mi2yhmhbph16yfsq-01DKTH87JpHSbzWb1dSs9p2J`

### Branch Naming Conventions

- **Feature branches:** `claude/<feature-description>-<session-id>`
- **Main branch:** *[TO BE DOCUMENTED: main, master, develop?]*
- **Release branches:** *[TO BE DOCUMENTED: versioning strategy]*

### Commit Message Guidelines

Follow conventional commit format:

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, no logic change)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

**Examples:**
```
feat(auth): add user authentication system

Implemented JWT-based authentication with refresh tokens.
Added login and logout endpoints.

Closes #123
```

```
fix(api): handle null response in user endpoint

Added null check to prevent crash when user data is missing.
```

### Git Commands Reference

**Push to feature branch:**
```bash
git push -u origin <branch-name>
```

**Fetch specific branch:**
```bash
git fetch origin <branch-name>
```

**Pull with retry logic:**
```bash
git pull origin <branch-name>
# Retry with exponential backoff (2s, 4s, 8s, 16s) if network errors occur
```

---

## Testing Strategy

### Current State
*No testing framework configured yet*

### Future Testing Approach
*[TO BE DOCUMENTED: Testing frameworks, coverage requirements, test organization]*

Suggested structure:
- **Unit Tests:** Test individual functions/components
- **Integration Tests:** Test component interactions
- **E2E Tests:** Test complete user workflows
- **Coverage Target:** *[e.g., 80% code coverage]*

### Running Tests
*[TO BE DOCUMENTED: Commands to run different test suites]*

```bash
# Example commands (update as applicable)
# npm test              # Run all tests
# npm run test:unit     # Run unit tests only
# npm run test:coverage # Generate coverage report
```

---

## Code Patterns

### Common Patterns to Follow
*[TO BE DOCUMENTED: Recurring patterns in the codebase]*

Examples to document:
- Error handling approach
- Async operation patterns
- State management patterns
- API request/response handling
- Logging conventions

### Anti-Patterns to Avoid
*[TO BE DOCUMENTED: Patterns that should be avoided]*

Examples:
- Avoid global state where possible
- Don't commit sensitive data (API keys, passwords)
- Avoid deeply nested callbacks
- Don't skip error handling

---

## AI Assistant Guidelines

### General Principles

1. **Understand Before Acting**
   - Read relevant code before making changes
   - Check for existing patterns and follow them
   - Look for similar implementations in the codebase

2. **Incremental Development**
   - Make small, focused changes
   - Test after each significant change
   - Commit logical units of work

3. **Code Quality**
   - Follow existing code style
   - Write clear, self-documenting code
   - Add comments for complex logic
   - Handle errors appropriately

4. **Communication**
   - Explain significant changes
   - Ask for clarification when requirements are unclear
   - Document assumptions

### Task Workflow

1. **Planning Phase**
   - Use TodoWrite to create task breakdown
   - Identify files that need to be modified
   - Check for dependencies and impacts

2. **Implementation Phase**
   - Read existing code first
   - Make changes incrementally
   - Update todos as you progress
   - Test changes

3. **Completion Phase**
   - Verify all requirements met
   - Run tests if available
   - Commit changes with clear messages
   - Push to the correct branch

### Security Considerations

- Never commit sensitive data (.env files, API keys, credentials)
- Validate and sanitize user input
- Avoid command injection vulnerabilities
- Use parameterized queries for databases
- Follow OWASP Top 10 guidelines
- Implement proper authentication and authorization

### File Operations

- **Read before Write:** Always read existing files before modifying them
- **Prefer Edit over Write:** Use Edit tool for existing files
- **Minimal File Creation:** Only create new files when absolutely necessary
- **No Unnecessary Docs:** Don't create README or markdown files unless explicitly requested

---

## Common Tasks

### Adding a New Feature

1. Create todo list for the feature
2. Read related code to understand context
3. Implement feature following existing patterns
4. Add tests (when testing framework is available)
5. Update documentation if needed
6. Commit and push changes

### Fixing a Bug

1. Reproduce the issue
2. Identify the root cause
3. Read surrounding code for context
4. Implement fix
5. Add regression test
6. Verify fix works
7. Commit with clear message explaining the fix

### Refactoring Code

1. Understand current implementation
2. Identify improvement opportunities
3. Make changes incrementally
4. Ensure tests still pass
5. Verify functionality unchanged
6. Commit refactoring separately from feature changes

### Updating Dependencies

*[TO BE DOCUMENTED: Process for updating dependencies]*

---

## Troubleshooting

### Common Issues

#### Git Push Failures

**Issue:** 403 error on push

**Solutions:**
- Verify branch name starts with `claude/` and ends with session ID
- Check network connectivity
- Retry with exponential backoff (2s, 4s, 8s, 16s)

**Issue:** Merge conflicts

**Solutions:**
- Fetch latest changes: `git fetch origin`
- Review conflicts carefully
- Test after resolving conflicts

#### Build Issues

*[TO BE DOCUMENTED: Common build problems and solutions]*

#### Test Failures

*[TO BE DOCUMENTED: Debugging test failures]*

---

## Maintenance Notes

### Updating This Document

This CLAUDE.md file should be updated when:
- Project structure changes significantly
- New conventions are established
- New tools or frameworks are added
- Testing strategy is implemented
- Build process is configured
- Deployment process is defined

**How to Update:**
1. Make changes to relevant sections
2. Update "Last Updated" date at the top
3. Consider adding version number if major changes
4. Commit with message: `docs(claude): update CLAUDE.md - <brief description>`

### Document Ownership

This document is maintained for AI assistants (primarily Claude Code) but should be reviewed and approved by human developers to ensure accuracy and alignment with project goals.

---

## Quick Reference

### Essential Commands

**Git Operations:**
```bash
# Check status
git status

# View recent commits
git log --oneline -10

# Create and switch to new branch
git checkout -b <branch-name>

# Stage and commit
git add <files>
git commit -m "type(scope): message"

# Push changes
git push -u origin <branch-name>
```

### Project-Specific Commands

*[TO BE DOCUMENTED: Add project-specific commands as they're established]*

---

## Additional Resources

### External Documentation
*[TO BE DOCUMENTED: Links to relevant documentation]*

### Related Repositories
*[TO BE DOCUMENTED: Related projects or dependencies]*

### Contact Information
*[TO BE DOCUMENTED: How to get help or ask questions]*

---

**Note:** This document is a living guide that will evolve with the repository. Sections marked with *[TO BE DOCUMENTED]* should be filled in as the project develops and conventions are established.
