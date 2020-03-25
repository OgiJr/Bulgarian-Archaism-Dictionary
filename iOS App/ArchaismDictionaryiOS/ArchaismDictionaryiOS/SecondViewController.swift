//
//  SecondViewController.swift
//  ArchaismDictionaryiOS
//
//  Created by Ognian Trajanov on 25.03.20.
//  Copyright © 2020 AR Learn. All rights reserved.
//

import UIKit
import Foundation

class SecondViewController: UIViewController {

    var dataBase = [[String]]()
    
    struct Welcome: Codable {
        let property1: [Property1]

        enum CodingKeys: String, CodingKey {
            case property1 = "Property1"
        }
    }

    struct Property1: Codable {
        let type: String
        let version, comment, name, database: String?
        let data: [Datum]?
    }

    struct Datum: Codable {
        let the0, the1: String
        let the2: JSONNull?
        let the3, id, word: String
        let synonym: JSONNull?
        let definition: String

        enum CodingKeys: String, CodingKey {
            case the0 = "0"
            case the1 = "1"
            case the2 = "2"
            case the3 = "3"
            case id, word, synonym, definition
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        DictionaryManager();
    }
    
    func DictionaryManager()
    {
        let urlString = "http://www.archaismdictionary.bg/json_manager.php"
        guard let url = URL(string: urlString) else{
            print("Error")
            return
        }
        do {
            let jsonString = try String(contentsOf: url)
            let data = Data(jsonString.utf8)
            let decoder = JSONDecoder()
            let dataParsed = try decoder.decode(Welcome.self, from: data)
            
            let size = dataParsed.property1[2].data?.count
            let count = 0...size! - 1
            
            dataBase = Array(repeating: Array(repeating: "default", count: 2), count: size!)
            
            for number in count{
                      dataBase[number][0] = dataParsed.property1[2].data?[number].word ?? "<no word>"
                      dataBase[number][1] = dataParsed.property1[2].data?[number].definition ?? "<no word>"
            }
        }
        catch let error as NSError{
            print("Error: \(error)")
        }
    }
    
    func SearchInDictionary(input: String) -> String{
        let size = dataBase.count
        let count = 0...size - 1
        var result: String = ""
        
        for number in count{
            if(input.lowercased() == dataBase[number][0].lowercased()){
                result = dataBase[number][0] + " " + dataBase[number][1]
            }
            else if result.isEmpty{
                    if(input.lowercased().contains(dataBase[number][0].lowercased())){
                        result = dataBase[number][0] + " " + dataBase[number][1]
                    }
                }
}
        return result
    }
    
class JSONNull: Codable, Hashable {

    public static func == (lhs: JSONNull, rhs: JSONNull) -> Bool {
        return true
    }

    public var hashValue: Int {
        return 0
    }

    public init() {}

    public required init(from decoder: Decoder) throws {
        let container = try decoder.singleValueContainer()
        if !container.decodeNil() {
            throw DecodingError.typeMismatch(JSONNull.self, DecodingError.Context(codingPath: decoder.codingPath, debugDescription: "Wrong type for JSONNull"))
        }
    }

    public func encode(to encoder: Encoder) throws {
        var container = encoder.singleValueContainer()
        try container.encodeNil()
    }
}
}
