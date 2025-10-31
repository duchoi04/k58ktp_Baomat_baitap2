# k58ktp_Baomat_baitap2
nội dung bài tập 2
BÀI TẬP VỀ NHÀ – MÔN: AN TOÀN VÀ BẢO MẬT THÔNG TIN

Chủ đề: Chữ ký số trong file PDF 

Giảng viên: Đỗ Duy Cốp 

Thời điểm giao: 2025-10-24 11:45 

Đối tượng áp dụng: Toàn bộ sv lớp học phần 58KTPM 

Hạn nộp: Sv upload tất cả lên github trước 2025-10-31 23:59:59 

--- 

Hoàng Đức Hội

MSV: K225480106085


I. MÔ TẢ CHUNG

Sinh viên thực hiện báo cáo và thực hành: phân tích và hiện thực việc nhúng, xác 

thực chữ ký số trong file PDF. 

Phải nêu rõ chuẩn tham chiếu (PDF 1.7 / PDF 2.0, PAdES/ETSI) và sử dụng công cụ 

thực thi (ví dụ iText7, OpenSSL, PyPDF, pdf-lib). 

--- 

II. CÁC YÊU CẦU CỤ THỂ

1. CẤU TRÚC PDF LIÊN QUAN CHỮ KÝ (TÓM TẮT)
2. 
Các object liên quan chính:

Catalog (Root): entry /AcroForm trỏ tới AcroForm object.

Pages tree: Catalog → /Pages → Page objects.

Page object: contains /Contents, /Resources.

AcroForm: chứa /Fields — danh sách field, trong đó có Signature field (widget).

Signature Field (Widget): field type Sig (AcroForm field) — appearance (visual) optional.

Signature Dictionary (/Sig): chứa các entry quan trọng: /Filter, /SubFilter, /ByteRange, /Contents, /M, /Name, /Reference.

/Contents: chứa blob PKCS#7 (DER), thường hex-encoded hoặc binary in a string with reserved length.

/ByteRange: array of 4 integers [offset0, length0, offset1, length1] chỉ phần file được hash (khi ghi signature, vùng /Contents được loại trừ).

/M: signing time as text (human readable, không phải timestamp token).

Incremental update: việc thêm chữ ký thường ghi như incremental PDF update — nội dung mới appended ở cuối file; dùng để phát hiện sửa đổi.

DSS (Document Security Store) — PAdES: optional object chứa Certs, OCSP, CRL, và timestamp tokens để hỗ trợ LTV (Long-Term Validation).


sơ đồ object:


<img width="1839" height="603" alt="Screenshot 2025-10-29 232413" src="https://github.com/user-attachments/assets/cc146a19-be14-4444-851c-2a895fc2bcf6" />

2. THỜI GIAN KÝ ĐƯỢC LƯU Ở ĐÂU?
Các vị trí có thể lưu thông tin thời gian ký:

2.1 /M trong Signature dictionary

Dạng: text (ví dụ: D:20251024...Z) - chỉ là thông tin mô tả, không đảm bảo tính pháp lý cho timestamp.

2.2 Timestamp token (RFC 3161)

Được đưa vào như attribute timeStampToken trong PKCS#7 (signedAttributes) — đây là một timestamp do Time Stamping Authority (TSA) cấp, có tính pháp lý và chống được replay (vì chứa hash của tệp và signature time).

2.3 Document timestamp object (PAdES)

PAdES định nghĩa cách nhúng document-level timestamp objects.

2.4 DSS

Nếu DSS chứa token timestamp, điều này hỗ trợ LTV.

Khác biệt /M vs RFC3161 timestamp

/M: chỉ text lưu thời điểm; có thể do client tự gán; dễ bị giả mạo (phụ thuộc vào chữ ký để bảo vệ nhưng bản thân /M không đảm bảo).

RFC3161 timestamp token: do TSA ký, chứa hash và thời gian; thường được chèn trong PKCS#7 signed attributes; có giá trị chứng thực thời gian mạnh hơn.


3) Các bước tạo và lưu chữ ký trong PDF (đã có private RSA)
- Viết script/code thực hiện tuần tự:

 1. Chuẩn bị file PDF gốc:
 
 
 <img width="1305" height="725" alt="Screenshot 2025-10-31 215217" src="https://github.com/user-attachments/assets/0fb81a31-3926-4a2a-abbc-03c13e1bc8a8" />

 tạo môi trường ảo mới bằng Conda:

<img width="1101" height="629" alt="Screenshot 2025-10-31 215159" src="https://github.com/user-attachments/assets/d9123993-daf3-4e32-af41-cca3aba6d9e7" />

tạo môi trường ảo để chạy code an toàn, độc lập với hệ thống:


<img width="794" height="418" alt="Screenshot 2025-10-31 215549" src="https://github.com/user-attachments/assets/561f0109-0fea-442b-8a1c-5ceb11223e0a" />


<img width="1073" height="558" alt="Screenshot 2025-10-31 215819" src="https://github.com/user-attachments/assets/569a598d-d1da-4f58-88f1-b3b5f45dd9cb" />

<img width="1113" height="335" alt="Screenshot 2025-10-31 215848" src="https://github.com/user-attachments/assets/c86a3a27-d133-4a44-9fcb-d05d9b57be54" />

 cài thư viện cryptography:

<img width="1104" height="560" alt="Screenshot 2025-10-31 220059" src="https://github.com/user-attachments/assets/ea4bb0a9-e689-43d7-9e08-9934e306cc5a" />

tạo khóa bí mật RSA 2048-bit: 

<img width="1089" height="291" alt="Screenshot 2025-10-31 220300" src="https://github.com/user-attachments/assets/9afcacd7-6b14-468b-98b7-77d34aeb7950" />

tạo chứng chỉ tự ký (self-signed certificate): 

<img width="1089" height="291" alt="Screenshot 2025-10-31 220300" src="https://github.com/user-attachments/assets/e3052202-6fb4-42e4-9efd-1a42e66d3fe3" />

 cài thư viện pyHanko: 
 

<img width="1096" height="501" alt="Screenshot 2025-10-31 220453" src="https://github.com/user-attachments/assets/941ded35-60aa-4336-8a7e-79336a4a4cf3" />

Đã ký PDF thành công! File lưu tại: D:\baomat\signed.pdf” : 

<img width="853" height="225" alt="Screenshot 2025-10-31 220620" src="https://github.com/user-attachments/assets/8a1c3247-e315-461c-b917-8f5f5b468164" />

chữ ký được ký vào file pdf: 

<img width="1174" height="656" alt="image" src="https://github.com/user-attachments/assets/35e9f506-e4f8-42cf-a03f-1c2ad97f11e3" />


4. Xác thực chữ ký trên PDF đã ký:

<img width="1057" height="237" alt="Screenshot 2025-10-31 220836" src="https://github.com/user-attachments/assets/7835ba20-9a32-434f-8920-fdf466913d4d" />


