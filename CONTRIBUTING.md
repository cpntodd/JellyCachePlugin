# Contributing

We welcome contributions to the Jelly Cache Manager plugin! This document outlines the process for contributing.

## How to Contribute

1. **Fork the Repository**: Create a fork of this repository on GitHub.

2. **Clone Your Fork**:

   ```bash
   git clone https://github.com/cpntodd/JellyCachePlugin.git
   cd jelly-cache
   ```

3. **Create a Branch**: Create a feature branch for your changes.

   ```bash
   git checkout -b feature/your-feature-name
   ```

4. **Make Changes**: Implement your feature or fix.

5. **Test Your Changes**: Ensure the plugin builds and works correctly.
   - Build with `dotnet build`
   - Test in a development Jellyfin environment

6. **Commit Changes**:

   ```bash
   git add .
   git commit -m "Add your descriptive commit message"
   ```

7. **Push to Your Fork**:

   ```bash
   git push origin feature/your-feature-name
   ```

8. **Create a Pull Request**: Open a pull request on the main repository.

## Development Guidelines

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add comments for complex logic
- Keep methods small and focused

### Testing

- Test your changes thoroughly
- Ensure no regressions in existing functionality
- Test on different Jellyfin versions if possible

### Documentation

- Update README.md if adding new features
- Update CHANGELOG.md for significant changes
- Add code comments where necessary

## Reporting Issues

If you find a bug or have a feature request:

1. Check existing issues to avoid duplicates
2. Create a new issue with:
   - Clear title and description
   - Steps to reproduce (for bugs)
   - Expected vs actual behavior
   - Jellyfin version and environment details

## Code of Conduct

Please be respectful and constructive in all interactions. We aim to maintain a positive community.

## License

By contributing, you agree that your contributions will be licensed under the same MIT License that covers the project.
