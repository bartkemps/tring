namespace Examples;

using System.Diagnostics.CodeAnalysis;
using Ternary3;
using Ternary3.IO;

/// <summary>
/// Demonstrates the Ternary3.IO namespace functionality for working with ternary streams.
/// Shows how to use Int3TStream, MemoryInt3TStream, ByteToInt3TStream, and Int3TToByteStream.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IODemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine($"\r\n\r\n{nameof(IODemo)}");

        // EXAMPLE 1: Basic MemoryInt3TStream operations
        // -------------------------------------------
        Console.WriteLine("Example 1: Basic MemoryInt3TStream operations");
        
        var memoryStream = new MemoryInt3TStream();
        
        // Write some Int3T values
        await memoryStream.WriteInt3TAsync(1);   // ter001
        await memoryStream.WriteInt3TAsync(-1);  // terTTT
        await memoryStream.WriteInt3TAsync(0);   // ter000
        await memoryStream.WriteInt3TAsync(13);  // ter111
        
        Console.WriteLine($"  Written 4 Int3T values to memory stream");
        Console.WriteLine($"  Stream length: {memoryStream.Length} Int3T values");
        Console.WriteLine($"  Stream position: {memoryStream.Position}");
        
        // Reset position to read from the beginning
        memoryStream.Position = 0;
        
        // Read values back one by one
        Console.WriteLine("  Reading values back:");
        for (var i = 0; i < 4; i++)
        {
            var value = await memoryStream.ReadInt3TAsync();
            Console.WriteLine($"    Position {i}: {value} (Int3T: {((Int3T)value):ter})");
        }

        // EXAMPLE 2: Bulk read/write operations
        // ------------------------------------
        Console.WriteLine("\nExample 2: Bulk read/write operations");
        
        var bulkStream = new MemoryInt3TStream();
        var writeBuffer = new Int3T[] 
        { 
            ter101,  // 10
            terT0T,  // -10
            ter111,  // 13
            terTTT,  // -13
            ter010,  // 3
            terT1T   // -3
        };
        
        // Write array of values
        await bulkStream.WriteAsync(writeBuffer, 0, writeBuffer.Length);
        Console.WriteLine($"  Written {writeBuffer.Length} values: [{string.Join(", ", writeBuffer.Select(v => $"{(int)v}"))}]");
        
        // Read back into a new buffer
        bulkStream.Position = 0;
        var readBuffer = new Int3T[writeBuffer.Length];
        var bytesRead = await bulkStream.ReadAsync(readBuffer, 0, readBuffer.Length);
        
        Console.WriteLine($"  Read {bytesRead} values: [{string.Join(", ", readBuffer.Select(v => $"{(int)v}"))}]");
        Console.WriteLine($"  Values match: {writeBuffer.SequenceEqual(readBuffer)}");

        // EXAMPLE 3: Stream seeking and positioning
        // ---------------------------------------
        Console.WriteLine("\nExample 3: Stream seeking and positioning");
        
        // Seek to middle of stream
        var newPosition = await bulkStream.SeekAsync(2, SeekOrigin.Begin);
        Console.WriteLine($"  Sought to position 2, actual position: {newPosition}");
        
        // Read from middle
        var middleValue = await bulkStream.ReadInt3TAsync();
        Console.WriteLine($"  Value at position 2: {middleValue} (Int3T: {((Int3T)middleValue):ter})");
        
        // Seek to end and read (expecting EndOfStreamException)
        await bulkStream.SeekAsync(0, SeekOrigin.End);
        try
        {
            var endValue = await bulkStream.ReadInt3TAsync();
            Console.WriteLine($"  Reading past end returns: {endValue}");
        }
        catch (EndOfStreamException ex)
        {
            Console.WriteLine($"  Reading past end throws: {ex.Message}");
        }

        // EXAMPLE 4: Converting ternary to binary and back
        // ----------------------------------------------
        Console.WriteLine("\nExample 4: Converting ternary to binary and back");
        
        // Create source ternary data
        var sourceStream = new MemoryInt3TStream();
        var ternaryData = new Int3T[] { ter101, terT10, ter011, terTT1 };
        await sourceStream.WriteAsync(ternaryData, 0, ternaryData.Length);
        sourceStream.Position = 0;
        
        Console.WriteLine($"  Original ternary data: [{string.Join(", ", ternaryData.Select(v => $"{(int)v} ({v:ter})"))}]");
        
        // Convert to bytes using Int3TToByteStream
        using var byteStream = new Int3TToByteStream(sourceStream, true, true);
        var binaryData = new List<byte>();
        var buffer = new byte[1024];
        var totalBytesRead = 0;
        
        while (true)
        {
            var bytesRead2 = await byteStream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead2 == 0) break;
            
            binaryData.AddRange(buffer.Take(bytesRead2));
            totalBytesRead += bytesRead2;
        }
        
        Console.WriteLine($"  Converted to {totalBytesRead} bytes: [{string.Join(", ", binaryData.Select(b => $"0x{b:X2}"))}]");
        
        // Convert back to ternary using ByteToInt3TStream
        var binaryMemoryStream = new MemoryStream(binaryData.ToArray());
        await using var backToTernaryStream = new ByteToInt3TStream(binaryMemoryStream, true, true);
        
        var recoveredData = new Int3T[ternaryData.Length];
        var tritsRead = await backToTernaryStream.ReadAsync(recoveredData, 0, recoveredData.Length);
        
        Console.WriteLine($"  Recovered {tritsRead} ternary values: [{string.Join(", ", recoveredData.Select(v => $"{(int)v} ({v:ter})"))}]");
        Console.WriteLine($"  Round-trip successful: {ternaryData.SequenceEqual(recoveredData)}");

        // EXAMPLE 5: File I/O with ternary data
        // ------------------------------------
        Console.WriteLine("\nExample 5: File I/O with ternary data");
        
        var tempFileName = Path.GetTempFileName();
        try
        {
            await using (var fileStream = File.Create(tempFileName))
            {
                await using var ternaryFileStream = new ByteToInt3TStream(fileStream);
                await using var ternaryWriter = new TernaryWriter(ternaryFileStream);
                await ternaryWriter.WriteAsync((Int27T)ter111000111TTT000TTT111000111);
            }
            
            Console.WriteLine($"  Written ternary values to file: {tempFileName}");
            Console.WriteLine($"  File size: {new FileInfo(tempFileName).Length} bytes");
            
            // Read ternary data back from file
            await using (var fileStream = File.OpenRead(tempFileName))
            {
                await using var converter = new ByteToInt3TStream(fileStream, true, true);
                var readFileData = new Int3T[9];
                var fileTritsRead = await converter.ReadAsync(readFileData, 0, readFileData.Length);
                
                Console.WriteLine($"  Read {fileTritsRead} ternary values from file");
                Console.WriteLine($"  Read:     [{string.Join(", ", readFileData.Select(v => $"{(int)v}"))}]");
            }
        }
        finally
        {
            if (File.Exists(tempFileName))
                File.Delete(tempFileName);
        }

        // EXAMPLE 6: Stream capacity and expansion
        // --------------------------------------
        Console.WriteLine("\nExample 6: Stream capacity and expansion");
        
        var capacityStream = new MemoryInt3TStream(2); // Start with capacity of 2
        Console.WriteLine($"  Initial capacity: 2"); // Remove Capacity property access
        
        // Write more data than initial capacity
        var expansionData = new Int3T[] { ter001, ter010, ter011, ter100, ter101 };
        await capacityStream.WriteAsync(expansionData, 0, expansionData.Length);
        
        Console.WriteLine($"  After writing {expansionData.Length} values:");
        Console.WriteLine($"  Length: {capacityStream.Length}");
        Console.WriteLine($"  Stream auto-expanded to accommodate data");
        
        // Convert to array
        var arrayData = capacityStream.ToArray();
        Console.WriteLine($"  ToArray() result: [{string.Join(", ", arrayData.Select(v => $"{(int)v}"))}]");

        // EXAMPLE 7: Error handling and edge cases
        // ---------------------------------------
        Console.WriteLine("\nExample 7: Error handling and edge cases");
        
        var readOnlyStream = new MemoryInt3TStream([ter001, ter010], false);
        Console.WriteLine($"  Created read-only stream with 2 values");
        Console.WriteLine($"  CanRead: {readOnlyStream.CanRead}, CanWrite: {readOnlyStream.CanWrite}");
        
        try
        {
            await readOnlyStream.WriteInt3TAsync(ter011);
            Console.WriteLine("  ERROR: Should not be able to write to read-only stream!");
        }
        catch (NotSupportedException)
        {
            Console.WriteLine("  Correctly prevented writing to read-only stream");
        }
        
        // Reading beyond stream length
        readOnlyStream.Position = 0;
        for (var i = 0; i < 4; i++) // Try to read 4 values from 2-value stream
        {
            try
            {
                var value = await readOnlyStream.ReadInt3TAsync();
                Console.WriteLine($"  Position {i}: {value}");
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine($"  Position {i}: End of stream reached (EndOfStreamException)");
                break; // Exit the loop once we hit the end of stream
            }
        }
        
        Console.WriteLine("IODemo completed successfully!");
    }
}
