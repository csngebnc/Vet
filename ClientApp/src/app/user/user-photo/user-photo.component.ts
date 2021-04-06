import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { base64ToFile, ImageCroppedEvent } from 'ngx-image-cropper';
import { VetUserDto } from 'src/app/_models/vetuserdto';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-user-photo',
  templateUrl: './user-photo.component.html',
  styleUrls: ['./user-photo.component.css']
})
export class UserPhotoComponent implements OnInit {

  constructor(private userService: UserService, private ngbModal: NgbActiveModal) { }

  imageChangedEvent: any = '';
  image;
  @Input() userId;
  @Input() photoPath;

  user: VetUserDto;

  ngOnInit(): void {
  }

  fileChangeEvent(event: any): void { this.imageChangedEvent = event; }
  imageCropped(event: ImageCroppedEvent) {
    this.image = base64ToFile(event.base64);
  }
  imageLoaded() { }
  cropperReady() { }
  loadImageFailed() { }

  submit() {
    let formData = new FormData();
    formData.append('profilePhoto', this.image);
    this.userService.updatePhoto(formData).subscribe(() => { this.ngbModal.close() });
  }
}
