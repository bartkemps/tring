```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.5472/23H2/2023Update/SunValley3)
Intel Core i7-10750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.100-preview.4.25258.110
  [Host]     : .NET 8.0.17 (8.0.1725.26602), X64 RyuJIT AVX2
  Job-KYJTVO : .NET 8.0.17 (8.0.1725.26602), X64 RyuJIT AVX2

IterationCount=2  WarmupCount=1  

```
| Method            | Mean      | Error | StdDev    | Gen0   | Allocated |
|------------------ |----------:|------:|----------:|-------:|----------:|
| Int32_To32Trits   |  4.337 μs |    NA | 0.0111 μs |      - |         - |
| Int64_To64Trits   |  5.237 μs |    NA | 0.0145 μs |      - |         - |
| UInt32_To32Trits  |  3.778 μs |    NA | 0.0794 μs |      - |         - |
| UInt64_To64Trits  |  4.674 μs |    NA | 0.0002 μs |      - |         - |
| ToInt32_FromTrits | 13.006 μs |    NA | 0.0530 μs | 0.1221 |     848 B |
| ToInt64_FromTrits |  9.048 μs |    NA | 0.1879 μs | 0.2594 |    1648 B |
