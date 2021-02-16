# 2.0.0-rc.4
- MicroElements.Metadata updated to version 7.0.0

# 1.3.0
- MicroElements.Functional updated to version 1.10.0
- MicroElements.Metadata updated to version 5.2.0

# 1.2.0
- MicroElements.Functional updated to version 1.6.0
- MicroElements.Metadata updated to version 5.1.0

# 1.1.0
- MicroElements.Functional updated to version 1.3.0
- MicroElements.Metadata updated to version 4.3.0

# 1.0.0
- MicroElements.Functional updated to version 1.0.0
- MicroElements.Metadata updated to version 3.5.0

# 0.9.0
- Fixed bug with removing key on cache enumeration while cache item is creating with long factory method

# 0.8.0
- IsSuccess and IsEmpty moved to extensions

# 0.7.0
- Extracted interfaces ICacheResult and ICacheResult<T> from CacheResult<>
- Added ICacheSection.GetCacheEntryUntyped
- Added GetAllEntriesUntyped extensions

# 0.6.0
- Added optional configure action to ICacheSetction.Set

# 0.5.0
- CacheResult extensions for usability

# 0.4.0
- Breaking: ValidationContext, ErrorHandleContext
- ICacheSection.GetCacheEntry
- CacheResult.GetValueOrDefault, CacheResult.GetValueOrThrow
- GetAllEntries extension method

# 0.3.0
- GetAllValues, GetAllKeyValues
- CacheResult throws CacheException in implicit conversion if result has error
- CacheResult.Error enriches with properties: SectionName, Key, Exception, ErrorMessage

# 0.2.0
- CacheSettings builder

# 0.1.0
- CacheManager

Full release notes can be found at: https://github.com/micro-elements/MicroElements.Data/blob/master/CHANGELOG.md
