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
    
    let session = AVCaptureSession()
    var camera : AVCaptureDevice?
    var cameraPreviewLayer: AVCaptureVideoPreviewLayer?
    var cameraCaptureOutput: AVCapturePhotoOutput?
    
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
        cameraPreviewLayer?.frame = view.bounds
        cameraPreviewLayer?.connection?.videoOrientation = AVCaptureVideoOrientation.portrait
        
        view.layer.insertSublayer(cameraPreviewLayer!, at: 0)
        session.startRunning()
    }
    
    func TakePicture(){
        
        let settings = AVCapturePhotoSettings()
        settings.flashMode = .auto
        cameraCaptureOutput?.capturePhoto(with: settings, delegate: self)
    }
 
    func DisplayPhoto(capturedPhoto: UIImage){
        let imagePreviewController = storyboard?.instantiateViewController(withIdentifier: "ImagePreviewController") as! ImagePreview
        imagePreviewController.capturedImage = capturedPhoto
        navigationController?.pushViewController(imagePreviewController, animated: true)
    }
    
    @IBAction func Button(_ sender: UIButton) {
        TakePicture()
    }
    
    @IBAction func Назад(_ sender: UIButton) {
        self.dismiss(animated: true, completion: nil)
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
                    DisplayPhoto(capturedPhoto: finalImage)
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
