using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class Player {
    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Email { get; set; }

    [FirestoreProperty]
    public string Character { get; set; }

    [FirestoreProperty]
    public int Level { get; set; }

    [FirestoreProperty]
    public int Experience { get; set; }

    [FirestoreProperty]
    public List<string> Builds { get; set; } = new List<string>();

    public Dictionary<string, object> ConvertToDictionary() {
        return new Dictionary<string, object> {
            {"Id", Id},
            {"Name", Name},
            {"Email", Email},
            {"Character", Character},
            {"Level", Level},
            {"Experience", Experience},
            {"Builds", Builds}
        };
    }

    public void SetFromDictionary(Dictionary<string, object> data) {
        Id = (int)data["Id"];
        Name = (string)data["Name"];
        Email = (string)data["Email"];
        Character = (string)data["Character"];
        Level = (int)data["Level"];
        Experience = (int)data["Experience"];
        Builds = (List<string>)data["Builds"];
    }
}