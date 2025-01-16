# Gate Pass Management System built using ASP.NET MVC

---

## 🚀 **Overview**

This system is designed for Sri Lanka Telecom (SLT) to efficiently manage and track gate passes for items/assets moving in and out of organizational premises. It supports authentication, request workflows, item management, and robust security features.

---

## 📂 **Table of Contents**

1. [Application Purpose](#application-purpose)
2. [Key Features](#key-features)
3. [Database Structure](#database-structure)
4. [Request Flow](#request-flow)
5. [Security Features](#security-features)
6. [How to Use](#how-to-use)
7. [Controllers, Views, & Models Documentation](#controllers-views--models)
8. [Database Models Documentation](#database-models-documentation)

---

## 📌 **Application Purpose**

This is an enterprise Gate Pass Management System for:

- Issuing and tracking gate passes.
- Handling item/asset movement.

---

## 🌟 **Key Features**

### **Authentication & User Management:**

- Integrates SLT’s OneIdentity server.
- Supports SLT and non-SLT employees.
- Role-based permissions.

### **Gate Pass Request Workflow:**

- **Stages:**
  - Verification
  - Executive Approval
  - Dispatch
  - Receipt/Return Tracking
- Status tracking and comments handling.

### **Item Management:**

- Manage item categories and locations.
- CSV upload for bulk operations.

---

## 🗂️ **Database Structure**

| Table          | Purpose                   |
| -------------- | ------------------------- |
| UserInfo       | Stores user details       |
| ExecutiveInfo  | Executive officer info    |
| ItemCategory   | Manages item categories   |
| SystemLocation | Stores location data      |
| WorkProgress   | Tracks request statuses   |
| Request Tables | Tracks gate pass requests |

---

## 🔄 **Request Flow**

1. **New Request Creation** (_NewRequestController_)
2. **Verification** (_VerifyController_)
3. **Executive Approval** (_ExeApproveController_)
4. **Dispatch Processing** (_DispatchController_)
5. **Receipt Confirmation** (_MyReceiptController_)
6. **Return Tracking** (_ItemTrackerController_)

---

## 🛡️ **Security Features**

- JWT-based authentication.
- Role-based access control.
- Secure API endpoints.
- Session management.

---

## 🧑‍💻 **How to Use**

1. Log in using SLT’s authentication system.
2. Create new requests.
3. Follow approval workflow.
4. Track statuses through _MyRequest_.
5. Manage dispatches and receipts.
6. Track and manage returnable items.

---

## 📘 **Controllers, Views, & Models Documentation**

### ### DispatchController

- **Purpose:** Manage dispatch processes.
- **Functions:**
  - `DispatchVerifyDetails(id)` -> _DispatchVerifyDetails.cshtml_
  - `Dispatch()` -> _Dispatch.cshtml_
  - `Reject(requestRefNo, comment)`

### ExeApproveController

- **Purpose:** Handle executive approvals.
- **Functions:**
  - `ExeApprove()` -> _ExeApprove.cshtml_
  - `PendingDetails(id)` -> _PendingDetails.cshtml_

(_...continues for all controllers..._)

---

## 🗄️ **Database Models Documentation**

### **DispatchModel**

- **Table:** GatePass_Dispatch
- **Columns:**
  - Request_ref_no (PK)
  - Item_category
  - Item_description
  - Quantity
  - Status

### **ExeApproveModel**

- **Table:** GatePass_ExeApprove
- **Columns:**
  - Request_ref_no (PK)
  - Requester_service_no
  - Status

(_...continues for all models..._)

---
