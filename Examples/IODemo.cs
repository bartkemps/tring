namespace Examples;

using Ternary3;
using Ternary3.IO;

/// <summary>
/// Demonstrates the Ternary3.IO namespace functionality for working with ternary streams.
/// Shows how to use Int3TStream, MemoryInt3TStream, ByteToInt3TStream, and Int3TToByteStream.
/// </summary>
public static partial class IODemo
{
    public static async Task Run()
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
        
        // Reset position to read from beginning
        memoryStream.Position = 0;
        
        // Read values back one by one
        Console.WriteLine("  Reading values back:");
        for (int i = 0; i < 4; i++)
        {
            int value = await memoryStream.ReadInt3TAsync();
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
        int bytesRead = await bulkStream.ReadAsync(readBuffer, 0, readBuffer.Length);
        
        Console.WriteLine($"  Read {bytesRead} values: [{string.Join(", ", readBuffer.Select(v => $"{(int)v}"))}]");
        Console.WriteLine($"  Values match: {writeBuffer.SequenceEqual(readBuffer)}");

        // EXAMPLE 3: Stream seeking and positioning
        // ---------------------------------------
        Console.WriteLine("\nExample 3: Stream seeking and positioning");
        
        // Seek to middle of stream
        long newPosition = await bulkStream.SeekAsync(2, SeekOrigin.Begin);
        Console.WriteLine($"  Sought to position 2, actual position: {newPosition}");
        
        // Read from middle
        int middleValue = await bulkStream.ReadInt3TAsync();
        Console.WriteLine($"  Value at position 2: {middleValue} (Int3T: {((Int3T)middleValue):ter})");
        
        // Seek to end and read (should return -1 for end of stream)
        await bulkStream.SeekAsync(0, SeekOrigin.End);
        int endValue = await bulkStream.ReadInt3TAsync();
        Console.WriteLine($"  Reading past end returns: {endValue}");

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
        int totalBytesRead = 0;
        
        while (true)
        {
            int bytesRead2 = await byteStream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead2 == 0) break;
            
            binaryData.AddRange(buffer.Take(bytesRead2));
            totalBytesRead += bytesRead2;
        }
        
        Console.WriteLine($"  Converted to {totalBytesRead} bytes: [{string.Join(", ", binaryData.Select(b => $"0x{b:X2}"))}]");
        
        // Convert back to ternary using ByteToInt3TStream
        var binaryMemoryStream = new MemoryStream(binaryData.ToArray());
        await using var backToTernaryStream = new ByteToInt3TStream(binaryMemoryStream, true, true);
        
        var recoveredData = new Int3T[ternaryData.Length];
        int tritsRead = await backToTernaryStream.ReadAsync(recoveredData, 0, recoveredData.Length);
        
        Console.WriteLine($"  Recovered {tritsRead} ternary values: [{string.Join(", ", recoveredData.Select(v => $"{(int)v} ({v:ter})"))}]");
        Console.WriteLine($"  Round-trip successful: {ternaryData.SequenceEqual(recoveredData)}");

        // EXAMPLE 5: File I/O with ternary data
        // ------------------------------------
        Console.WriteLine("\nExample 5: File I/O with ternary data");
        
        string tempFileName = Path.GetTempFileName();
        try
        {
            // Write ternary data to file
            var fileData = new Int3T[] 
            { 
                5,   // ter01T
                -8,  // terT1T
                12,  // ter110
                -4   // terT11
            };
            
            using (var fileStream = File.Create(tempFileName))
            {
                var ternaryFileStream = new MemoryInt3TStream();
                await ternaryFileStream.WriteAsync(fileData, 0, fileData.Length);
                ternaryFileStream.Position = 0;
                
                using var converter = new Int3TToByteStream(ternaryFileStream, true, true);
                await converter.CopyToAsync(fileStream);
            }
            
            Console.WriteLine($"  Written {fileData.Length} ternary values to file: {tempFileName}");
            Console.WriteLine($"  File size: {new FileInfo(tempFileName).Length} bytes");
            
            // Read ternary data back from file
            using (var fileStream = File.OpenRead(tempFileName))
            {
                await using var converter = new ByteToInt3TStream(fileStream, true, true);
                var readFileData = new Int3T[fileData.Length];
                int fileTritsRead = await converter.ReadAsync(readFileData, 0, readFileData.Length);
                
                Console.WriteLine($"  Read {fileTritsRead} ternary values from file");
                Console.WriteLine($"  Original: [{string.Join(", ", fileData.Select(v => $"{(int)v}"))}]");
                Console.WriteLine($"  Read:     [{string.Join(", ", readFileData.Select(v => $"{(int)v}"))}]");
                Console.WriteLine($"  File round-trip successful: {fileData.SequenceEqual(readFileData)}");
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
        
        var readOnlyStream = new MemoryInt3TStream(new Int3T[] { ter001, ter010 }, false);
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
        for (int i = 0; i < 4; i++) // Try to read 4 values from 2-value stream
        {
            int value = await readOnlyStream.ReadInt3TAsync();
            if (value == -1)
            {
                Console.WriteLine($"  Position {i}: End of stream reached (returned -1)");
            }
            else
            {
                Console.WriteLine($"  Position {i}: {value}");
            }
        }
        
        Console.WriteLine("IODemo completed successfully!");
    }
}
