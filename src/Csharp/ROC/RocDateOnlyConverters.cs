using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utilities.ROC;

#region TypeConverter

/// <summary>
/// 提供 <see cref="RocDateOnly"/> 與其他型別之間轉換的 TypeConverter
/// </summary>
public class RocDateOnlyTypeConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string)
            || sourceType == typeof(DateOnly)
            || sourceType == typeof(DateTime)
            || base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string)
            || destinationType == typeof(DateOnly)
            || destinationType == typeof(DateTime)
            || base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            string s => RocDateOnly.Parse(s),
            DateOnly d => RocDateOnly.FromDateOnly(d),
            DateTime dt => RocDateOnly.FromDateTime(dt),
            _ => base.ConvertFrom(context, culture, value)
        };
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is RocDateOnly roc)
        {
            if (destinationType == typeof(string))
                return roc.ToString();
            if (destinationType == typeof(DateOnly))
                return roc.CeDateOnly;
            if (destinationType == typeof(DateTime))
                return roc.ToDateTime(TimeOnly.MinValue);
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}

#endregion

#region System.Text.Json

/// <summary>
/// 提供 <see cref="RocDateOnly"/> 的 System.Text.Json 序列化與反序列化
/// </summary>
public class RocDateOnlyJsonConverter : JsonConverter<RocDateOnly>
{
    private readonly string? _format;

    /// <summary>
    /// 使用預設格式 (yyy/MM/dd) 建立轉換器
    /// </summary>
    public RocDateOnlyJsonConverter() : this(null) { }

    /// <summary>
    /// 使用指定格式建立轉換器
    /// </summary>
    /// <param name="format">日期格式字串</param>
    public RocDateOnlyJsonConverter(string? format)
    {
        _format = format;
    }

    /// <inheritdoc/>
    public override RocDateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrEmpty(str))
            return default;

        return RocDateOnly.Parse(str);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, RocDateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_format is null ? value.ToString() : value.ToString(_format));
    }
}

/// <summary>
/// 提供 <see cref="RocDateOnly"/> 的 JsonConverterFactory
/// </summary>
public class RocDateOnlyJsonConverterFactory : JsonConverterFactory
{
    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(RocDateOnly);
    }

    /// <inheritdoc/>
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new RocDateOnlyJsonConverter();
    }
}

#endregion

#region Dapper

/// <summary>
/// 提供 <see cref="RocDateOnly"/> 的 Dapper TypeHandler
/// </summary>
/// <remarks>
/// 使用方式: 在應用程式啟動時呼叫 <c>SqlMapper.AddTypeHandler(new RocDateOnlyDapperHandler());</c>
/// </remarks>
public class RocDateOnlyDapperHandler : Dapper.SqlMapper.TypeHandler<RocDateOnly>
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
        /// 以民國日期字串格式儲存 (yyy/MM/dd)
        /// </summary>
        RocString,

        /// <summary>
        /// 以整數格式儲存 (yyyMMdd)
        /// </summary>
        Integer
    }

    private readonly StorageFormat _format;

    /// <summary>
    /// 使用預設格式 (DateTime) 建立 Handler
    /// </summary>
    public RocDateOnlyDapperHandler() : this(StorageFormat.DateTime) { }

    /// <summary>
    /// 使用指定格式建立 Handler
    /// </summary>
    /// <param name="format">儲存格式</param>
    public RocDateOnlyDapperHandler(StorageFormat format)
    {
        _format = format;
    }

    /// <inheritdoc/>
    public override RocDateOnly Parse(object value)
    {
        return value switch
        {
            DateTime dt => RocDateOnly.FromDateTime(dt),
            DateOnly d => RocDateOnly.FromDateOnly(d),
            string s => RocDateOnly.Parse(s),
            int i => RocDateOnly.Parse(i.ToString()),
            long l => RocDateOnly.Parse(l.ToString()),
            _ => throw new ArgumentException($"無法將 {value.GetType().Name} 轉換為 RocDateOnly", nameof(value))
        };
    }

    /// <inheritdoc/>
    public override void SetValue(System.Data.IDbDataParameter parameter, RocDateOnly value)
    {
        parameter.Value = _format switch
        {
            StorageFormat.DateTime => value.ToDateTime(TimeOnly.MinValue),
            StorageFormat.RocString => value.ToString(),
            StorageFormat.Integer => int.Parse(value.ToString("yyyMMdd")),
            _ => value.ToDateTime(TimeOnly.MinValue)
        };

        parameter.DbType = _format switch
        {
            StorageFormat.DateTime => System.Data.DbType.Date,
            StorageFormat.RocString => System.Data.DbType.String,
            StorageFormat.Integer => System.Data.DbType.Int32,
            _ => System.Data.DbType.Date
        };
    }
}

#endregion

#region Entity Framework Core

/// <summary>
/// 提供 <see cref="RocDateOnly"/> 與 <see cref="DateOnly"/> 之間轉換的 Entity Framework Core ValueConverter
/// </summary>
/// <remarks>
/// 使用方式:
/// <code>
/// protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
/// {
///     configurationBuilder.Properties&lt;RocDateOnly&gt;()
///         .HaveConversion&lt;RocDateOnlyToDateOnlyConverter&gt;();
/// }
/// </code>
/// 或在 OnModelCreating 中:
/// <code>
/// modelBuilder.Entity&lt;MyEntity&gt;()
///     .Property(e => e.RocDate)
///     .HasConversion(new RocDateOnlyToDateOnlyConverter());
/// </code>
/// </remarks>
public class RocDateOnlyToDateOnlyConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<RocDateOnly, DateOnly>
{
    /// <summary>
    /// 建立轉換器實例
    /// </summary>
    public RocDateOnlyToDateOnlyConverter()
        : base(
            roc => roc.CeDateOnly,
            ce => RocDateOnly.FromDateOnly(ce))
    { }
}

/// <summary>
/// 提供 <see cref="RocDateOnly"/> 與 <see cref="DateTime"/> 之間轉換的 Entity Framework Core ValueConverter
/// </summary>
public class RocDateOnlyToDateTimeConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<RocDateOnly, DateTime>
{
    /// <summary>
    /// 建立轉換器實例
    /// </summary>
    public RocDateOnlyToDateTimeConverter()
        : base(
            roc => roc.ToDateTime(TimeOnly.MinValue),
            dt => RocDateOnly.FromDateTime(dt))
    { }
}

/// <summary>
/// 提供 <see cref="RocDateOnly"/> 與民國日期字串之間轉換的 Entity Framework Core ValueConverter
/// </summary>
public class RocDateOnlyToStringConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<RocDateOnly, string>
{
    /// <summary>
    /// 建立轉換器實例 (預設格式: yyy/MM/dd)
    /// </summary>
    public RocDateOnlyToStringConverter()
        : this(null)
    { }

    /// <summary>
    /// 使用指定格式建立轉換器實例
    /// </summary>
    /// <param name="format">日期格式字串</param>
    public RocDateOnlyToStringConverter(string? format)
        : base(
            roc => format == null ? roc.ToString() : roc.ToString(format),
            s => RocDateOnly.Parse(s))
    { }
}

/// <summary>
/// 提供 <see cref="RocDateOnly"/> 與整數 (yyyMMdd) 之間轉換的 Entity Framework Core ValueConverter
/// </summary>
public class RocDateOnlyToIntConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<RocDateOnly, int>
{
    /// <summary>
    /// 建立轉換器實例
    /// </summary>
    public RocDateOnlyToIntConverter()
        : base(
            roc => roc.Year * 10000 + roc.Month * 100 + roc.Day,
            i => new RocDateOnly(i / 10000, (i / 100) % 100, i % 100))
    { }
}

#endregion
