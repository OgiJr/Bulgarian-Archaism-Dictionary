//
//  FirstViewController.swift
//  ArchaismDictionary
//
//  Created by Jordan Draganchev on 24.03.20.
//  Copyright © 2020 AR Learn. All rights reserved.
//

import UIKit

class FirstViewController: UIViewController {

    var dataBase = [[String]]()
    
    struct WordData
    {
        var id: Int = 0
        var word: String = ""
        var synonym: String = ""
        var definition: String = ""
    }
    
    struct Table
    {
        var type: String = ""
        var version: String = ""
        var comment: String = ""
        var name: String = ""
        var database: String = ""
        var data: WordData
    }
    
    struct JSONClass
    {
        var Property1: Table
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
            print(jsonString)
        }
        catch let error as NSError{
            print("Error: \(error)")
        }
    }
}
