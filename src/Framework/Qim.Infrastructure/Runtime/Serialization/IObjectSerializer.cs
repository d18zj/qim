namespace Qim.Runtime.Serialization
{
    /// <summary>
    ///     Represents that the implemented classes are object serializers.
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        ///     Serializes an object into a byte stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The byte stream which contains the serialized data.</returns>
        byte[] SerializeBinary<TObject>(TObject obj);

        /// <summary>
        ///     Deserializes an object from the given byte stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="bytes"></param>
        /// <returns>The deserialized object.</returns>
        TObject Deserialize<TObject>(byte[] bytes);

        /// <summary>
        ///     Serializes an object into a string.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The string which contains the serialized data.</returns>
        string Serialize<TObject>(TObject obj);

        /// <summary>
        ///     Deserializes an object from the string.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="serializedString"></param>
        /// <returns></returns>
        TObject Deserialize<TObject>(string serializedString);
    }
}