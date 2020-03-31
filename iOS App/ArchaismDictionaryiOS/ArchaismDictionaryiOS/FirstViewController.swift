//
//  FirstViewController.swift
//  ArchaismDictionaryiOS
//
//  Created by Ognian Trajanov on 25.03.20.
//  Copyright © 2020 AR Learn. All rights reserved.
//

import UIKit
import AVFoundation

class FirstViewController: UIViewController {
    
    @IBOutlet weak var ImageView: UIImageView!
    @IBOutlet weak var Значение: UILabel!
    @IBOutlet weak var Label: UILabel!
    
    let session = AVCaptureSession()
    var camera : AVCaptureDevice?
    var cameraPreviewLayer: AVCaptureVideoPreviewLayer?
    var cameraCaptureOutput: AVCapturePhotoOutput?
    var ocrResult: String = ""
    var finalResult: String = ""
    
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
        InitializeCaptureSession()
    }
    
    func InitializeCaptureSession(){
        
        session.sessionPreset = AVCaptureSession.Preset.high
        camera = AVCaptureDevice.default(for: AVMediaType.video)
        
        do{
        let cameraCaptureInput = try AVCaptureDeviceInput(device: camera!)
            cameraCaptureOutput = AVCapturePhotoOutput()
            
            session.addInput(cameraCaptureInput)
            session.addOutput(cameraCaptureOutput!)
        } catch{
            print(error.localizedDescription)
        }
        
        cameraPreviewLayer = AVCaptureVideoPreviewLayer(session: session)
        cameraPreviewLayer?.videoGravity = AVLayerVideoGravity.resizeAspectFill
        cameraPreviewLayer?.frame = self.view.bounds
        cameraPreviewLayer?.connection?.videoOrientation = AVCaptureVideoOrientation.portrait
        
        view.layer.insertSublayer(cameraPreviewLayer!, at: 0)
        session.startRunning()
    }
    
    override func viewWillTransition(to size: CGSize, with coordinator: UIViewControllerTransitionCoordinator) {
        if UIDevice.current.orientation.isLandscape {
            cameraPreviewLayer?.frame = self.view.bounds
                   cameraPreviewLayer?.connection?.videoOrientation = AVCaptureVideoOrientation.landscapeLeft
                           }
    }
        
    func TakePicture(){
        
        let settings = AVCapturePhotoSettings()
        settings.flashMode = .auto
        cameraCaptureOutput?.capturePhoto(with: settings, delegate: self)
    }
    
    
    func ScanPhoto(capturedPhoto: UIImage){
            
        
        
            if(ocrResult != ""){
            
                let result = SearchInDictionary(input: ocrResult)
            
                if !result.isEmpty{
                
                let resultArr = result.split{$0 == " "}.map(String.init)
                Значение.text = resultArr[0].capitalizingFirstLetter()
                var array = ""
                let definitionLength = 1...resultArr.count - 1
                for i in definitionLength{
                    if(i == 1){
                    array += resultArr[i].capitalizingFirstLetter() + " "
                    }
                    else{
                    array += resultArr[i] + " "
                    }
                }
                Label.text = array
            }
        }
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
            print(result)
            return result
}
    
    @IBAction func Button(_ sender: UIButton) {
        TakePicture()
    }
}

extension FirstViewController : AVCapturePhotoCaptureDelegate
{
    public func photoOutput(_ output: AVCapturePhotoOutput, didFinishProcessingPhoto photoSampleBuffer: CMSampleBuffer?, previewPhoto previewPhotoSampleBuffer: CMSampleBuffer?, resolvedSettings: AVCaptureResolvedPhotoSettings, bracketSettings: AVCaptureBracketedStillImageSettings?, error: Error?) {
        if let unwrappedError = error{
               print(unwrappedError.localizedDescription)
           }
           else{
            if let sampleBuffer = photoSampleBuffer, let dataImage = AVCapturePhotoOutput.jpegPhotoDataRepresentation(forJPEGSampleBuffer: sampleBuffer, previewPhotoSampleBuffer: previewPhotoSampleBuffer){
                
                if let finalImage = UIImage(data: dataImage){
                    ScanPhoto(capturedPhoto: finalImage)
                }
            }
           }
    }
}

class ImagePreview: FirstViewController{
    var capturedImage: UIImage?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        ImageView.image = capturedImage
    }
}
