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
