using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utilities.ROC;

#region TypeConverter

/// <summary>
/// 提供 <see cref="RocDateTime"/> 與其他型別之間轉換的 TypeConverter
/// </summary>
public class RocDateTimeTypeConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string)
            || sourceType == typeof(DateTime)
            || sourceType == typeof(DateTimeOffset)
            || base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string)
            || destinationType == typeof(DateTime)
            || destinationType == typeof(DateTimeOffset)
            || base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            string s => RocDateTime.Parse(s),
            DateTime dt => RocDateTime.FromDateTime(dt),
            DateTimeOffset dto => RocDateTime.FromDateTime(dto.DateTime),
            _ => base.ConvertFrom(context, culture, value)
        };
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is RocDateTime roc)
        {
            if (destinationType == typeof(string))
                return roc.ToString();
            if (destinationType == typeof(DateTime))
                return roc.ToDateTime();
            if (destinationType == typeof(DateTimeOffset))
                return new DateTimeOffset(roc.ToDateTime());
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}

#endregion

#region System.Text.Json

/// <summary>
/// 提供 <see cref="RocDateTime"/> 的 System.Text.Json 序列化與反序列化
/// </summary>
public class RocDateTimeJsonConverter : JsonConverter<RocDateTime>
{
    private readonly string? _format;

    /// <summary>
    /// 使用預設格式 (yyy/MM/dd HH:mm:ss) 建立轉換器
    /// </summary>
    public RocDateTimeJsonConverter() : this(null) { }

    /// <summary>
    /// 使用指定格式建立轉換器
    /// </summary>
    /// <param name="format">日期時間格式字串</param>
    public RocDateTimeJsonConverter(string? format)
    {
        _format = format;
    }

    /// <inheritdoc/>
    public override RocDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrEmpty(str))
            return default;

        return RocDateTime.Parse(str);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, RocDateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_format is null ? value.ToString() : value.ToString(_format));
    }
}

/// <summary>
/// 提供 <see cref="RocDateTime"/> 的 JsonConverterFactory
/// </summary>
public class RocDateTimeJsonConverterFactory : JsonConverterFactory
{
    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(RocDateTime);
    }

    /// <inheritdoc/>
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new RocDateTimeJsonConverter();
    }
}

#endregion

#region Dapper

/// <summary>
/// 提供 <see cref="RocDateTime"/> 的 Dapper TypeHandler
/// </summary>
/// <remarks>
/// 使用方式: 在應用程式啟動時呼叫 <c>SqlMapper.AddTypeHandler(new RocDateTimeDapperHandler());</c>
/// </remarks>
public class RocDateTimeDapperHandler : Dapper.SqlMapper.TypeHandler<RocDateTime>
{
    /// <summary>
    /// 資料庫儲存格式
    /// </summary>
    public enum StorageFormat
    {
        /// <summary>
        /// 以西元 DateTime 格式儲存 (預設)
        /// </summary>
        DateTime,

        /// <summary>
        /// 以民國日期時間字串格式儲存 (yyy/MM/dd HH:mm:ss)
        /// </summary>
        RocString
    }

    private readonly StorageFormat _format;
    private readonly string? _stringFormat;

    /// <summary>
    /// 使用預設格式 (DateTime) 建立 Handler
    /// </summary>
    public RocDateTimeDapperHandler() : this(StorageFormat.DateTime, null) { }

    /// <summary>
    /// 使用指定格式建立 Handler
    /// </summary>
    /// <param name="format">儲存格式</param>
    /// <param name="stringFormat">當 format 為 RocString 時使用的格式字串</param>
    public RocDateTimeDapperHandler(StorageFormat format, string? stringFormat = null)
    {
        _format = format;
        _stringFormat = stringFormat;
    }

    /// <inheritdoc/>
    public override RocDateTime Parse(object value)
    {
        return value switch
        {
            DateTime dt => RocDateTime.FromDateTime(dt),
            DateTimeOffset dto => RocDateTime.FromDateTime(dto.DateTime),
            string s => RocDateTime.Parse(s),
            _ => throw new ArgumentException($"無法將 {value.GetType().Name} 轉換為 RocDateTime", nameof(value))
        };
    }

    /// <inheritdoc/>
    public override void SetValue(System.Data.IDbDataParameter parameter, RocDateTime value)
    {
        parameter.Value = _format switch
        {
            StorageFormat.DateTime => value.ToDateTime(),
            StorageFormat.RocString => _stringFormat is null ? value.ToString() : value.ToString(_stringFormat),
            _ => value.ToDateTime()
        };

        parameter.DbType = _format switch
        {
            StorageFormat.DateTime => System.Data.DbType.DateTime,
            StorageFormat.RocString => System.Data.DbType.String,
            _ => System.Data.DbType.DateTime
        };
    }
}

#endregion

#region Entity Framework Core

/// <summary>
/// 提供 <see cref="RocDateTime"/> 與 <see cref="DateTime"/> 之間轉換的 Entity Framework Core ValueConverter
/// </summary>
/// <remarks>
/// 使用方式:
/// <code>
/// protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
/// {
///     configurationBuilder.Properties&lt;RocDateTime&gt;()
///         .HaveConversion&lt;RocDateTimeToDateTimeConverter&gt;();
/// }
/// </code>
/// 或在 OnModelCreating 中:
/// <code>
/// modelBuilder.Entity&lt;MyEntity&gt;()
///     .Property(e => e.RocDateTime)
///     .HasConversion(new RocDateTimeToDateTimeConverter());
/// </code>
/// </remarks>
public class RocDateTimeToDateTimeConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<RocDateTime, DateTime>
{
    /// <summary>
    /// 建立轉換器實例
    /// </summary>
    public RocDateTimeToDateTimeConverter()
        : base(
            roc => roc.ToDateTime(),
            dt => RocDateTime.FromDateTime(dt))
    { }
}

/// <summary>
/// 提供 <see cref="RocDateTime"/> 與 <see cref="DateTimeOffset"/> 之間轉換的 Entity Framework Core ValueConverter
/// </summary>
public class RocDateTimeToDateTimeOffsetConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<RocDateTime, DateTimeOffset>
{
    /// <summary>
    /// 建立轉換器實例
    /// </summary>
    public RocDateTimeToDateTimeOffsetConverter()
        : base(
            roc => new DateTimeOffset(roc.ToDateTime()),
            dto => RocDateTime.FromDateTime(dto.DateTime))
    { }
}

/// <summary>
/// 提供 <see cref="RocDateTime"/> 與民國日期時間字串之間轉換的 Entity Framework Core ValueConverter
/// </summary>
public class RocDateTimeToStringConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<RocDateTime, string>
{
    /// <summary>
    /// 建立轉換器實例 (預設格式: yyy/MM/dd HH:mm:ss)
    /// </summary>
    public RocDateTimeToStringConverter()
        : this(null)
    { }

    /// <summary>
    /// 使用指定格式建立轉換器實例
    /// </summary>
    /// <param name="format">日期時間格式字串</param>
    public RocDateTimeToStringConverter(string? format)
        : base(
            roc => format == null ? roc.ToString() : roc.ToString(format),
            s => RocDateTime.Parse(s))
    { }
}

/// <summary>
/// 提供 <see cref="RocDateTime"/> 與 Ticks (long) 之間轉換的 Entity Framework Core ValueConverter
/// </summary>
public class RocDateTimeToTicksConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<RocDateTime, long>
{
    /// <summary>
    /// 建立轉換器實例
    /// </summary>
    public RocDateTimeToTicksConverter()
        : base(
            roc => roc.Ticks,
            ticks => new RocDateTime(ticks))
    { }
}

#endregion
