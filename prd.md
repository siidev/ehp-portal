# Product Requirements Document (PRD)
# SSO Portal - Single Sign-On Authentication System

---

## 📋 Document Information

| **Document** | **Details** |
|---|---|
| **Product Name** | SSO Portal - Single Sign-On Authentication System |
| **Version** | v2.1 |
| **Document Version** | 2.1 |
| **Created Date** | October 8, 2025 |
| **Last Updated** | January 17, 2025 |
| **Status** | Production Ready |
| **Classification** | Internal |

---

## 🎯 Executive Summary

### Product Vision
SSO Portal adalah sistem autentikasi terpusat yang menyediakan Single Sign-On (SSO) untuk multiple aplikasi dalam ekosistem enterprise. Sistem ini dibangun menggunakan Blazor Server .NET 8 dengan built-in identity management, CAPTCHA protection, dan webhook-based integration untuk menyederhanakan akses pengguna across multiple aplikasi.

### Business Objectives
- **Centralized Authentication**: Satu sistem login untuk semua aplikasi perusahaan
- **Enhanced Security**: Implementasi CAPTCHA, role-based access control, dan secure token management
- **Improved User Experience**: Seamless login experience dengan modern UI menggunakan Masa Blazor
- **Administrative Efficiency**: Centralized user, application, dan vendor management
- **Self-Service**: Password reset capabilities untuk end users
- **Easy Integration**: Webhook-based integration dengan client applications
- **Multi-language Support**: Internationalization dengan i18n support

---

## 👥 Stakeholders

### Primary Stakeholders
- **IT Security Team**: Sistem keamanan dan compliance
- **System Administrators**: Management user dan aplikasi
- **End Users**: Employees yang menggunakan multiple aplikasi
- **Application Developers**: Tim yang mengintegrasikan aplikasi dengan SSO

### Secondary Stakeholders
- **Management**: Oversight dan approval untuk access controls
- **Help Desk**: User support untuk authentication issues

---

## 🏆 Success Metrics

### Key Performance Indicators (KPIs)
- **User Adoption Rate**: > 95% user adoption dalam 6 bulan
- **Login Success Rate**: > 99.5% successful authentication
- **System Uptime**: > 99.9% availability
- **Security Incidents**: Zero security breaches related to authentication
- **User Satisfaction**: > 4.5/5 rating untuk ease of use

### Technical Metrics
- **Response Time**: < 200ms untuk token validation
- **Token Generation**: < 500ms untuk SSO token creation
- **Database Performance**: < 100ms query response time
- **Page Load**: < 2 seconds untuk Blazor Server pages

---

## 🔍 Problem Statement

### Current Challenges
1. **Multiple Login Systems**: Users harus remember multiple credentials untuk different applications
2. **Security Risks**: Password fatigue leading to weak passwords atau password reuse
3. **Administrative Overhead**: Manual user management across multiple systems
4. **User Experience**: Friction dalam accessing multiple business applications
5. **Password Recovery**: Complex password reset procedures

### Impact
- **Productivity Loss**: Time wasted pada multiple logins
- **Security Vulnerabilities**: Inconsistent security implementations
- **Support Costs**: High volume of password reset requests
- **User Frustration**: Complicated authentication processes

---

## 🎯 Product Goals

### Primary Goals
1. **Unified Authentication**: Single login untuk all registered applications
2. **Enhanced Security**: CAPTCHA protection dan secure password management
3. **Centralized Management**: Admin interface untuk user dan application management
4. **Seamless Integration**: Token-based authentication untuk client applications
5. **Self-Service**: Password reset functionality untuk users

### Secondary Goals
1. **High Availability**: 99.9% uptime dengan proper architecture
2. **Scalability**: Support untuk 10,000+ concurrent users
3. **Performance**: Sub-second authentication response times
4. **Mobile Support**: Responsive interface
5. **Easy Integration**: Simple webhook-based integration untuk client apps

---

## 👤 User Personas

### 1. End User (Employee)
**Profile**: Regular employee yang menggunakan multiple business applications
- **Pain Points**: Multiple passwords, frequent re-authentication, forgotten passwords
- **Goals**: Quick access ke semua aplikasi dengan minimal friction, easy password recovery
- **Usage Pattern**: Daily login, multiple app access throughout day, occasional password resets

### 2. System Administrator
**Profile**: IT staff responsible untuk user dan system management
- **Pain Points**: Manual user provisioning, scattered access controls, password reset requests
- **Goals**: Efficient user management, centralized access control, reduced support tickets
- **Usage Pattern**: Daily admin tasks, periodic access reviews, user support

### 3. Application Developer
**Profile**: Developer integrating applications dengan SSO system
- **Pain Points**: Complex integration requirements, unclear documentation
- **Goals**: Easy integration, reliable authentication service
- **Usage Pattern**: Initial integration, occasional maintenance

---

## 🚀 Current Implementation Status

### ✅ Implemented Features (Production Ready)
- **Authentication System**: Custom authentication dengan password hashing
- **User Management**: Complete CRUD operations untuk users dengan role-based access
- **Application Management**: Register dan manage applications dengan webhook integration
- **Category Management**: Organize applications by categories
- **Vendor Management**: Manage vendor information dan relationships
- **SSO Token System**: Secure token generation dan validation untuk client apps
- **Webhook Integration**: Event-driven notifications untuk registered applications
- **Password Reset**: Email-based password reset functionality
- **CAPTCHA Protection**: Custom CAPTCHA implementation untuk login security
- **User Dashboard**: Modern UI untuk accessing authorized applications
- **Admin Dashboard**: Comprehensive admin interface untuk system management
- **Token Validation API**: HTTP endpoint untuk client applications validate SSO tokens
- **Multi-language Support**: i18n support dengan English dan Chinese
- **Responsive Design**: Mobile-friendly interface menggunakan Masa Blazor
- **Database Integration**: MySQL dengan Entity Framework Core
- **Email Service**: SMTP integration untuk notifications

### 🔄 In Progress Features
- **System Settings**: Configuration management untuk site settings
- **Account Settings**: User profile management
- **Advanced Security**: Enhanced security features

### 📋 Planned Features
- **Google reCAPTCHA Integration**: Optional Google reCAPTCHA v2/v3 support
- **Advanced Analytics**: User activity tracking dan reporting
- **API Rate Limiting**: Protection against abuse
- **Audit Logging**: Comprehensive audit trail

---

## ✨ Core Features

### 🔐 Authentication & Authorization

#### F1: Built-in Identity Management
**Priority**: P0 (Critical)
**Description**: Custom authentication system dengan password hashing
- **Capabilities**:
  - User authentication dengan secure password hashing
  - Session management via cookies
  - Role-based authorization (Admin/User)
  - Password policies enforcement
- **Acceptance Criteria**:
  - Users dapat login using credentials
  - Sessions managed securely dengan cookies
  - Roles properly assigned dan enforced
  - Password complexity requirements met

#### F2: CAPTCHA Protection
**Priority**: P0 (Critical)
**Description**: CAPTCHA verification untuk login attempts
- **Capabilities**:
  - **Phase 1**: Custom CAPTCHA implementation (text-based atau simple math)
  - **Phase 2**: Optional Google reCAPTCHA v2/v3 integration
  - CAPTCHA challenge pada login page
  - Protection against bot attacks
  - Configurable CAPTCHA type via settings
- **Acceptance Criteria**:
  - Custom CAPTCHA working dengan image generation
  - CAPTCHA displayed pada login page
  - Login blocked without valid CAPTCHA
  - Bot attacks effectively prevented
  - Architecture supports future Google reCAPTCHA integration
  - CAPTCHA type selectable via configuration

#### F3: Password Reset & Recovery
**Priority**: P0 (Critical)
**Description**: Self-service password reset functionality
- **Capabilities**:
  - Email-based password reset
  - Secure reset token generation
  - Token expiration management
  - Password history validation
- **Acceptance Criteria**:
  - Users dapat request password reset via email
  - Reset link sent to registered email
  - Reset tokens expire appropriately (24 hours)
  - New passwords cannot match previous password

#### F4: Single Sign-On (SSO) Token Management
**Priority**: P0 (Critical)
**Description**: Custom SSO token generation dan validation untuk registered apps
- **Capabilities**:
  - Automatic token generation after successful login
  - Token validation endpoint untuk client applications
  - Token refresh mechanism
  - Automatic token distribution via webhooks
- **Acceptance Criteria**:
  - SSO tokens generated automatically after login
  - Client apps dapat validate tokens via HTTP endpoint
  - Token refresh working properly
  - Webhook notifications sent to registered apps

### 👥 User Management

#### F5: User Administration
**Priority**: P0 (Critical)
**Description**: Comprehensive user management capabilities
- **Capabilities**:
  - User CRUD operations (Create, Read, Update, Delete)
  - Role assignment (admin/user)
  - User activation/deactivation (is_active toggle)
  - Password reset by admin
  - Bulk user operations
  - View active/inactive users
- **Acceptance Criteria**:
  - Admin dapat create, read, update, delete users
  - Role assignment working correctly
  - User status management functional (active/inactive)
  - Inactive users cannot login
  - Password policies enforced
  - Bulk operations efficient

#### F6: Application Access Control
**Priority**: P0 (Critical)
**Description**: Granular access control untuk applications
- **Capabilities**:
  - Grant/revoke app access untuk users
  - Bulk access management
  - Access control enforcement via tokens
  - View users per application
  - View applications per user
- **Acceptance Criteria**:
  - Admin dapat grant/revoke access per user per app
  - Users only see apps they have access to
  - Access control enforced by SSO tokens
  - Bulk operations available

### 📱 Application Management

#### F7: Application Registration
**Priority**: P0 (Critical)
**Description**: Registration dan management aplikasi dalam SSO ecosystem
- **Capabilities**:
  - App CRUD operations
  - Webhook URL configuration
  - App categorization
  - App status management (active/inactive)
  - Icon management
- **Acceptance Criteria**:
  - Admin dapat register new applications
  - Webhook configuration working
  - Apps properly categorized
  - App status reflected dalam user access
  - Icons displayed correctly

#### F8: Category Management
**Priority**: P1 (High)
**Description**: Organizational categorization untuk applications
- **Capabilities**:
  - Category CRUD operations
  - Icon management untuk categories
  - App assignment to categories
  - Category-based app grouping dalam UI
- **Acceptance Criteria**:
  - Categories dapat created dan managed
  - Apps properly grouped by category
  - Category icons displayed correctly
  - Category organization improves UX

### 🌐 Integration

#### F9: Token Validation Endpoint
**Priority**: P0 (Critical)
**Description**: HTTP endpoint untuk client applications validate SSO tokens
- **Capabilities**:
  - Simple HTTP GET/POST endpoint
  - Token validation logic
  - Return user info jika token valid
  - Return error jika token invalid/expired
- **Acceptance Criteria**:
  - Endpoint accessible via HTTP
  - Proper validation logic implemented
  - Response format consistent
  - Error handling proper

#### F10: Webhook System
**Priority**: P0 (Critical)
**Description**: Event-driven notifications untuk registered applications
- **Capabilities**:
  - Token creation webhooks
  - User logout webhooks
  - Access change webhooks
  - HMAC signature validation
  - Retry mechanism untuk failed webhooks
- **Acceptance Criteria**:
  - Webhooks sent untuk relevant events
  - HMAC signatures properly validated
  - Retry mechanism working
  - Webhook payload properly formatted

### 📊 Dashboard & Reporting

#### F11: User Dashboard
**Priority**: P1 (High)
**Description**: User-facing dashboard showing accessible applications
- **Capabilities**:
  - Display apps user has access to
  - Grouped by category
  - Quick access links
  - User profile management
- **Acceptance Criteria**:
  - Dashboard displays only authorized apps
  - Apps properly grouped
  - Links working correctly
  - Profile info editable

#### F12: Admin Dashboard
**Priority**: P1 (High)
**Description**: Comprehensive admin interface untuk system management
- **Capabilities**:
  - System overview (user count, app count, active sessions)
  - User management interface
  - App management interface
  - Category management interface
  - Quick actions panel
- **Acceptance Criteria**:
  - Dashboard provides clear system overview
  - Management interfaces intuitive dan efficient
  - Real-time data via SignalR
  - Responsive design

---

## 🏗️ Technical Architecture

### System Architecture (Monolithic Blazor Server)
```
┌─────────────────────────────────────────────────────────────┐
│                  BLAZOR SERVER MONOLITH                     │
│                                                             │
│  ┌──────────────────────────────────────────────────────┐  │
│  │              BLAZOR COMPONENTS                       │  │
│  │  ┌────────────┐  ┌────────────┐  ┌────────────┐    │  │
│  │  │   Login    │  │   Admin    │  │    User    │    │  │
│  │  │   Pages    │  │  Dashboard │  │  Dashboard │    │  │
│  │  └────────────┘  └────────────┘  └────────────┘    │  │
│  │                                                      │  │
│  │  ┌────────────┐  ┌────────────┐  ┌────────────┐    │  │
│  │  │    User    │  │    App     │  │  Category  │    │  │
│  │  │ Management │  │ Management │  │ Management │    │  │
│  │  └────────────┘  └────────────┘  └────────────┘    │  │
│  └──────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌──────────────────────────────────────────────────────┐  │
│  │              SERVICES LAYER                          │  │
│  │  ┌────────────────────────────────────────────────┐  │  │
│  │  │  AuthService │ UserService │ AppService        │  │  │
│  │  │  TokenService │ WebhookService │ EmailService  │  │  │
│  │  └────────────────────────────────────────────────┘  │  │
│  └──────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌──────────────────────────────────────────────────────┐  │
│  │              DATA ACCESS LAYER                       │  │
│  │  ┌────────────────────────────────────────────────┐  │  │
│  │  │  Entity Framework Core + MySQL                 │  │  │
│  │  │  DbContext, Repositories, Models               │  │  │
│  │  └────────────────────────────────────────────────┘  │  │
│  └──────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌──────────────────────────────────────────────────────┐  │
│  │         SIMPLE HTTP ENDPOINT (Token Validation)      │  │
│  │         /validate-token (untuk client apps)          │  │
│  └──────────────────────────────────────────────────────┘  │
│                                                             │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                     MYSQL DATABASE                          │
│  ┌─────────────────┐  ┌─────────────────┐                 │
│  │   Users, Apps   │  │   Tokens,       │                 │
│  │   Categories    │  │   Access Rights │                 │
│  └─────────────────┘  └─────────────────┘                 │
└─────────────────────────────────────────────────────────────┘
```

### Technology Stack

#### Framework & Runtime
- **Framework**: Blazor Server (.NET 8.0)
- **Runtime**: ASP.NET Core 8.0
- **Database**: MySQL 8.0+ dengan connection pooling
- **ORM**: Entity Framework Core dengan query optimization
- **Real-time**: SignalR (built-in dengan Blazor Server)
- **UI Framework**: Masa Blazor (Material Design components)

#### Authentication & Security
- **Authentication**: Cookie-based authentication dengan custom state provider
- **Password Hashing**: BCrypt dengan secure salt generation
- **CAPTCHA**: 
  - **Implemented**: Custom CAPTCHA service
    - Text-based challenge generation
    - Session-based validation dengan expiration
    - Image generation dengan distortion
  - **Future**: Google reCAPTCHA integration (optional)
    - reCAPTCHA v2 (checkbox)
    - reCAPTCHA v3 (invisible)
- **Token**: Custom secure token generation untuk SSO (64-character random string)
- **Webhook Security**: HMAC-SHA256 signatures untuk webhook validation
- **Session Management**: Secure cookie handling dengan sliding expiration

#### UI & Styling
- **UI Framework**: Masa Blazor (Material Design)
- **CSS Framework**: Masa Blazor built-in styling
- **Icons**: Material Design Icons (MDI)
- **State Management**: Cascading parameters, dependency injection, dan cookie storage
- **Responsive Design**: Mobile-first approach dengan breakpoint support
- **Internationalization**: i18n support dengan English dan Chinese

#### Integration
- **Email**: SMTP client untuk password reset emails
- **HTTP Client**: Simple endpoint untuk token validation
- **Webhooks**: HttpClient untuk webhook calls

#### Development Tools
- **IDE**: Visual Studio 2022 atau Rider
- **Version Control**: Git
- **Database Tools**: MySQL Workbench
- **Testing**: xUnit atau NUnit

---

## 📊 Data Model

### Current Database Schema

The application uses the following core entities with proper relationships and indexing:

#### Core Tables
- **users**: User management dengan role-based access
- **apps**: Application registration dengan webhook configuration
- **categories**: Application categorization
- **sso_portal_tokens**: SSO token management dengan expiration
- **user_apps_access**: Many-to-many relationship antara users dan applications
- **password_reset_tokens**: Secure password reset functionality
- **vendors**: Vendor management system
- **site_settings**: System configuration settings

#### Key Features
- **Soft Delete**: Implemented dengan `deleted_at` columns
- **Audit Trail**: `created_at` dan `updated_at` timestamps
- **Indexing**: Optimized indexes untuk performance
- **Foreign Keys**: Proper referential integrity
- **Soft Delete Support**: Cascade delete handling

### Core Entities

#### Users
```sql
CREATE TABLE users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    username VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL DEFAULT 'user',
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_username (username),
    INDEX idx_email (email),
    INDEX idx_role (role),
    INDEX idx_is_active (is_active)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
```

#### Apps (Applications)
```sql
CREATE TABLE apps (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    code VARCHAR(100) UNIQUE NOT NULL,
    description TEXT,
    url VARCHAR(500),
    webhook_url VARCHAR(500),
    webhook_secret VARCHAR(255),
    icon_url VARCHAR(500),
    category_id INT,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE SET NULL,
    INDEX idx_code (code),
    INDEX idx_category (category_id),
    INDEX idx_active (is_active)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
```

#### Categories
```sql
CREATE TABLE categories (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    code VARCHAR(100) UNIQUE NOT NULL,
    description TEXT,
    icon_url VARCHAR(500),
    display_order INT DEFAULT 0,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_code (code),
    INDEX idx_order (display_order)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
```

#### SSO Portal Tokens
```sql
CREATE TABLE sso_portal_tokens (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    app_id INT NOT NULL,
    token VARCHAR(500) NOT NULL UNIQUE,
    refresh_token VARCHAR(500),
    expires_at TIMESTAMP NOT NULL,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (app_id) REFERENCES apps(id) ON DELETE CASCADE,
    INDEX idx_token (token),
    INDEX idx_user (user_id),
    INDEX idx_app (app_id),
    INDEX idx_expires (expires_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
```

#### User App Access
```sql
CREATE TABLE user_apps_access (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    app_id INT NOT NULL,
    granted_by INT,
    granted_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    notes TEXT,
    UNIQUE KEY uk_user_app (user_id, app_id),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (app_id) REFERENCES apps(id) ON DELETE CASCADE,
    FOREIGN KEY (granted_by) REFERENCES users(id) ON DELETE SET NULL,
    INDEX idx_user (user_id),
    INDEX idx_app (app_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
```

#### Password Reset Tokens
```sql
CREATE TABLE password_reset_tokens (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    token VARCHAR(500) NOT NULL UNIQUE,
    expires_at TIMESTAMP NOT NULL,
    is_used TINYINT(1) NOT NULL DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    used_at TIMESTAMP NULL,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    INDEX idx_token (token),
    INDEX idx_user (user_id),
    INDEX idx_expires (expires_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
```

---

## 🔒 Security Requirements

### Authentication Security
- **CAPTCHA Protection**: 
  - **Custom CAPTCHA** (Phase 1):
    - Text-based atau simple math challenge
    - Random string generation (6 characters)
    - Image generation dengan noise/distortion
    - Session-based CAPTCHA storage
    - 5 minute expiration
  - **Google reCAPTCHA** (Phase 2 - Optional):
    - reCAPTCHA v2 untuk visible challenge
    - reCAPTCHA v3 untuk invisible scoring
    - Configurable via appsettings.json
- **Password Policy**: 
  - Minimum 8 characters
  - Must contain uppercase, lowercase, number, special character
  - Cannot be same as previous password
- **Session Management**: 
  - Secure cookie handling
  - Session timeout after 30 minutes inactivity
  - Automatic logout after 8 hours
- **Brute Force Protection**: Account lockout after 5 failed attempts
- **Password Reset**: 
  - Secure token-based reset
  - Token expires in 24 hours
  - One-time use only

### Authorization Security
- **Role-Based Access Control (RBAC)**: 
  - Admin: Full access to all features
  - User: Access only to authorized apps
- **Principle of Least Privilege**: Users only get access to apps they need
- **Access Validation**: Every page/action validates user permissions

### Data Security
- **Encryption in Transit**: HTTPS/TLS untuk all communications
- **Password Hashing**: BCrypt dengan salt (minimum cost factor 12)
- **Token Security**: Cryptographically secure random tokens
- **Database Security**: 
  - Parameterized queries (EF Core)
  - No raw SQL unless necessary
  - Connection string in secure configuration

### Application Security
- **Input Validation**: All user inputs validated dan sanitized
- **XSS Protection**: Blazor's built-in XSS protection
- **CSRF Protection**: Blazor's built-in CSRF tokens
- **SQL Injection**: Protected by EF Core parameterized queries
- **Webhook Security**: HMAC-SHA256 signature verification

---

## 🔌 Integration Requirements

### Current API Endpoints

#### 1. Token Validation Endpoint
**Endpoint**: `GET /validate-token`
**Purpose**: Validate SSO tokens untuk client applications
**Authentication**: None required
**Parameters**: 
- `token` (string, required): SSO token to validate

**Response Format**:
```json
{
  "valid": true,
  "user": {
    "id": 123,
    "username": "john.doe",
    "email": "john@example.com",
    "name": "John Doe",
    "role": "User"
  },
  "app": {
    "id": 5,
    "code": "APP001",
    "name": "My Application"
  },
  "expires_at": "2025-10-09T10:30:00Z"
}
```

#### 2. Webhook Integration
**Purpose**: Event-driven notifications untuk registered applications
**Events Supported**:
- `token.created`: When new SSO token is generated
- `user.logout`: When user logs out
- `access.revoked`: When user access is revoked

### Client Application Integration

#### Token Validation Endpoint
```
GET/POST /validate-token
Request: 
  - token: string (required)
  
Response (Success):
{
  "valid": true,
  "user": {
    "id": 123,
    "username": "john.doe",
    "email": "john@example.com",
    "name": "John Doe"
  },
  "app": {
    "id": 5,
    "code": "APP001",
    "name": "My Application"
  },
  "expires_at": "2025-10-09T10:30:00Z"
}

Response (Error):
{
  "valid": false,
  "error": "Token expired" | "Token invalid" | "Token not found"
}
```

#### Webhook Events
**1. Token Created Event**
```json
POST {webhook_url}
Headers:
  X-Signature: HMAC-SHA256 signature
  
Body:
{
  "event": "token.created",
  "timestamp": "2025-10-08T10:00:00Z",
  "data": {
    "token": "xxxxx",
    "user": {
      "id": 123,
      "username": "john.doe",
      "email": "john@example.com",
      "name": "John Doe"
    },
    "expires_at": "2025-10-09T10:00:00Z"
  }
}
```

**2. User Logout Event**
```json
POST {webhook_url}
Headers:
  X-Signature: HMAC-SHA256 signature
  
Body:
{
  "event": "user.logout",
  "timestamp": "2025-10-08T15:00:00Z",
  "data": {
    "user_id": 123,
    "tokens_invalidated": ["token1", "token2"]
  }
}
```

**3. Access Revoked Event**
```json
POST {webhook_url}
Headers:
  X-Signature: HMAC-SHA256 signature
  
Body:
{
  "event": "access.revoked",
  "timestamp": "2025-10-08T12:00:00Z",
  "data": {
    "user_id": 123,
    "app_id": 5,
    "reason": "Access removed by admin"
  }
}
```

#### Integration Steps untuk Client Apps
1. Register application dalam SSO Portal
2. Receive webhook URL dan secret
3. Implement token validation call
4. Implement webhook receiver dengan HMAC validation
5. Handle token lifecycle events

### Email Service Integration
- **SMTP Configuration**: 
  - Host, port, credentials via appsettings.json
  - Support untuk TLS/SSL
- **Email Templates**: 
  - Welcome email
  - Password reset email
  - Account locked email
- **Retry Logic**: Retry failed emails up to 3 times

### CAPTCHA Integration

#### Custom CAPTCHA (Phase 1)
**Implementation Details**:
- **Image Generation**:
  - Generate random 6-character alphanumeric string
  - Create image (200x80px) dengan text
  - Apply distortion/noise untuk prevent OCR
  - Return image sebagai base64 atau image endpoint
- **Storage**:
  - Store CAPTCHA value dalam session
  - Expire after 5 minutes
  - One-time use only
- **Validation**:
  - Case-insensitive comparison
  - Clear session after validation
  - Generate new CAPTCHA after failed attempt

**Example Flow**:
```
1. User visits login page
2. System generates random CAPTCHA: "A3bK9m"
3. Store dalam session dengan timestamp
4. Display image to user
5. User enters CAPTCHA
6. System validates:
   - Check if exists dalam session
   - Check if not expired (< 5 minutes)
   - Compare user input (case-insensitive)
   - Mark as used
7. If valid, proceed with login
8. If invalid, show error dan generate new CAPTCHA
```

#### Google reCAPTCHA Integration (Phase 2 - Future)
**Configuration Structure**:
```json
{
  "Captcha": {
    "Provider": "Custom", // "Custom" atau "GoogleReCaptcha"
    "Custom": {
      "Length": 6,
      "ExpirationMinutes": 5,
      "CaseSensitive": false
    },
    "GoogleReCaptcha": {
      "SiteKey": "your-site-key",
      "SecretKey": "your-secret-key",
      "Version": "v2", // "v2" atau "v3"
      "MinimumScore": 0.5 // untuk v3
    }
  }
}
```

**Integration Requirements**:
- Create `ICaptchaService` interface
- Implement `CustomCaptchaService`
- Implement `GoogleReCaptchaService` (future)
- Use factory pattern untuk select provider
- Easy switching via configuration

---

## 📱 User Experience Requirements

### Responsive Design
- **Mobile Support**: Full functionality pada mobile devices (768px+)
- **Tablet Support**: Optimized layout untuk tablets (1024px+)
- **Desktop**: Full features pada desktop (1280px+)
- **Cross-Browser**: Chrome, Firefox, Safari, Edge (latest 2 versions)

### User Interface Guidelines

#### Login Page
- Clean, minimal design
- CAPTCHA prominently displayed
- "Forgot Password" link clearly visible
- Error messages clear dan actionable
- Loading states during authentication

#### User Dashboard
- Card-based app display
- Grouped by category
- Search/filter functionality
- Quick launch buttons
- Profile dropdown menu

#### Admin Dashboard
- Overview cards (user count, app count, active sessions)
- Quick action buttons
- Recent activity list
- Navigation sidebar
- Breadcrumb navigation

### Accessibility
- **WCAG 2.1 Level AA**: Minimum compliance target
- **Keyboard Navigation**: Full keyboard accessibility
- **Screen Readers**: Proper ARIA labels
- **Color Contrast**: Minimum 4.5:1 ratio
- **Focus Indicators**: Clear focus states

---

## 📈 Performance Requirements

### Response Time
- **Page Load**: < 2 seconds untuk initial load
- **Component Interaction**: < 500ms untuk user actions
- **Token Validation**: < 200ms untuk validation endpoint
- **Database Queries**: < 100ms untuk standard queries
- **Login Process**: < 1 second total (excluding CAPTCHA time)

### Concurrent Users
- **Active Sessions**: Support 1,000+ concurrent users
- **SignalR Connections**: Handle 1,000+ WebSocket connections
- **Database Connections**: Connection pool of 50-100
- **Token Operations**: Generate/validate 100+ tokens per second

### Scalability Considerations
- **Database Indexing**: Proper indexes pada frequently queried columns
- **Connection Pooling**: Efficient EF Core connection management
- **SignalR Scaling**: Consider Redis backplane for multiple servers
- **Caching**: In-memory caching untuk static data (categories, app list)

---

## 🚀 Deployment & Infrastructure

### Current Production Configuration

#### Database Configuration
- **MySQL Server**: 4.154.188.173:3307
- **Database**: ssoportal
- **Connection Pooling**: Enabled dengan MaxPoolSize=100
- **Connection Timeout**: 15 seconds
- **Retry Policy**: 3 retries dengan 2-second delay

#### Email Configuration
- **SMTP Host**: smtp.office365.com:587
- **Authentication**: OAuth2 dengan app password
- **SSL/TLS**: Enabled
- **From Address**: media@siindonesia.com
- **From Name**: EHP SSO Portal

#### Application Settings
- **Base URL**: http://localhost:5100 (configurable)
- **Password Reset Expiry**: 24 hours
- **Session Timeout**: 8 hours dengan sliding expiration
- **Environment**: Production-ready dengan error handling

### Server Requirements

#### Production Server
- **OS**: Windows Server 2019+ atau Linux (Ubuntu 20.04+)
- **CPU**: 4+ cores
- **RAM**: 8GB minimum, 16GB recommended
- **Storage**: 50GB+ SSD
- **Network**: 100Mbps minimum

#### Database Server
- **MySQL**: 8.0+
- **CPU**: 4+ cores
- **RAM**: 16GB minimum
- **Storage**: 100GB+ SSD dengan RAID
- **Backup**: Daily automated backups

### Deployment Options

#### Option 1: IIS Deployment (Windows)
- Install .NET 8.0 Runtime
- Configure IIS application pool
- Deploy published application
- Configure SSL certificate
- Set up reverse proxy jika needed

#### Option 2: Self-Hosted (Linux)
- Install .NET 8.0 Runtime
- Deploy as systemd service
- Configure Nginx reverse proxy
- Set up SSL with Let's Encrypt
- Configure firewall rules

#### Option 3: Docker (Recommended)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY publish/ .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "SSOPortal.dll"]
```

### Configuration Management
- **appsettings.json**: Development settings
- **appsettings.Production.json**: Production settings
- **Environment Variables**: Sensitive data (connection strings, secrets)
- **Azure Key Vault**: Optional untuk enterprise deployments

### Monitoring & Logging

#### Application Logging
- **Serilog**: Structured logging
- **Log Levels**: Debug, Info, Warning, Error, Critical
- **Log Targets**: 
  - File (rolling)
  - Database (optional)
  - Console (development only)

#### Health Checks
- **/health**: Basic health check endpoint
- **/health/ready**: Readiness check (database connection)
- **/health/live**: Liveness check (application responding)

#### Performance Monitoring
- Response time tracking
- Database query performance
- SignalR connection monitoring
- Memory usage tracking

---

## 🧪 Testing Strategy

### Unit Testing
- **Framework**: xUnit
- **Mocking**: Moq
- **Coverage Target**: >80%
- **Focus Areas**:
  - Services (AuthService, TokenService, etc.)
  - Business logic
  - Data validation
  - Token generation/validation

### Integration Testing
- **Database Testing**: In-memory database atau test database
- **Component Testing**: bUnit untuk Blazor components
- **End-to-End**: Selenium atau Playwright
- **Focus Areas**:
  - User flows (login, logout, password reset)
  - Admin operations
  - Webhook delivery
  - Token validation

### Security Testing
- **Penetration Testing**: Annual security audit
- **Vulnerability Scanning**: Monthly automated scans
- **OWASP Top 10**: Regular testing against common vulnerabilities
- **Focus Areas**:
  - Authentication bypass attempts
  - SQL injection
  - XSS attacks
  - CSRF attacks
  - Session hijacking

### Performance Testing
- **Load Testing**: JMeter atau k6
- **Stress Testing**: Identify breaking points
- **Scenarios**:
  - 1,000 concurrent users
  - 10,000 token validations per minute
  - 100 simultaneous logins
  - Database query performance under load

---

## 📅 Release Plan

### ✅ Phase 1: Core Authentication (COMPLETED)
- ✅ Database schema design dan implementation
- ✅ User authentication system dengan custom state provider
- ✅ Password hashing dengan BCrypt
- ✅ Session management dengan cookie-based authentication
- ✅ Basic login/logout functionality
- ✅ User management CRUD dengan role-based access
- ✅ Soft delete implementation

### ✅ Phase 2: SSO & Integration (COMPLETED)
- ✅ SSO token generation dengan secure random strings
- ✅ Token validation endpoint (`/validate-token`)
- ✅ Webhook system implementation dengan HMAC signatures
- ✅ Application registration dengan webhook configuration
- ✅ User-app access control dengan many-to-many relationships
- ✅ HMAC signature validation untuk webhook security

### ✅ Phase 3: Security & Self-Service (COMPLETED)
- ✅ **Custom CAPTCHA implementation**
  - ✅ Text-based challenge generation
  - ✅ Session-based storage dengan expiration
  - ✅ Validation logic dengan case-insensitive comparison
- ✅ Password reset functionality dengan email integration
- ✅ Email service integration dengan SMTP
- ✅ Security hardening dengan proper validation
- ✅ Role-based authorization dengan Admin/User roles
- ✅ **CAPTCHA service interface** (ready untuk future Google integration)

### ✅ Phase 4: Admin Portal (COMPLETED)
- ✅ Admin dashboard dengan comprehensive overview
- ✅ User management interface dengan advanced filtering
- ✅ Application management interface dengan webhook configuration
- ✅ Category management dengan icon support
- ✅ Vendor management system
- ✅ Bulk operations dan search functionality
- ✅ System settings configuration

### ✅ Phase 5: Production Deployment (COMPLETED)
- ✅ Modern UI/UX dengan Masa Blazor Material Design
- ✅ Performance optimization dengan EF Core query optimization
- ✅ Comprehensive documentation dengan integration guides
- ✅ Production deployment dengan proper configuration
- ✅ Multi-language support (English/Chinese)
- ✅ Responsive design untuk mobile devices

### 🔄 Phase 6: Future Enhancements (PLANNED)
- 📋 **Google reCAPTCHA integration** (optional)
- 📋 Advanced analytics dan reporting
- 📋 API rate limiting
- 📋 Comprehensive audit logging
- 📋 Advanced security features
- 📋 Performance monitoring

---

## 🏗️ Current System Architecture

### Application Structure
```
SSOPortalX/
├── Data/
│   ├── Models/           # Entity models
│   ├── Services/         # Business logic services
│   ├── Security/         # Authentication & security
│   └── Webhook/         # Webhook integration
├── Pages/
│   ├── Authentication/   # Login/logout pages
│   ├── App/            # Application management
│   ├── Home/           # User dashboard
│   └── Others/         # Error pages, settings
├── Shared/             # Shared components
├── wwwroot/           # Static assets & i18n
└── Program.cs         # Application configuration
```

### Key Services
- **UserService**: User management operations
- **ApplicationService**: Application registration & management
- **SsoTokenService**: SSO token generation & validation
- **WebhookService**: Webhook delivery & management
- **EmailService**: SMTP email integration
- **CaptchaService**: CAPTCHA generation & validation
- **PasswordResetService**: Password reset functionality
- **SystemSettingsService**: Configuration management

### Database Schema
- **8 Core Tables**: Users, Applications, Categories, Tokens, Access, Vendors, Settings
- **Optimized Indexes**: Performance-focused database design
- **Soft Delete**: Data preservation with audit trails
- **Foreign Keys**: Referential integrity enforcement

### Security Implementation
- **Custom Authentication**: Cookie-based dengan sliding expiration
- **Password Security**: BCrypt hashing dengan salt
- **CAPTCHA Protection**: Custom implementation dengan session storage
- **Token Security**: 64-character random strings dengan expiration
- **Webhook Security**: HMAC-SHA256 signature validation
- **Input Validation**: Comprehensive validation pada semua inputs

### Integration Capabilities
- **Token Validation API**: `/validate-token` endpoint untuk client apps
- **Webhook System**: Event-driven notifications dengan retry mechanism
- **Email Integration**: SMTP dengan Office 365
- **Multi-language**: i18n support dengan English/Chinese
- **Responsive Design**: Mobile-first dengan Material Design

---

## 📞 Support & Maintenance

### Current Status
- **Production Ready**: Fully functional dengan comprehensive features
- **Documentation**: Complete integration guide tersedia
- **Testing**: Comprehensive testing strategy implemented
- **Monitoring**: Basic health checks dan logging
- **Security**: Production-grade security implementation

### Maintenance Requirements
- **Regular Updates**: .NET 8 security patches
- **Database Maintenance**: MySQL optimization dan backup
- **Email Service**: SMTP configuration monitoring
- **Security Audits**: Regular security reviews
- **Performance Monitoring**: Database query optimization

### Support Channels
- **Technical Documentation**: SSO_Integration_Guide.md
- **Code Repository**: Git-based version control
- **Issue Tracking**: GitHub issues atau internal system
- **Deployment**: Docker support dengan docker-compose.yml

---

**Last Updated**: January 17, 2025  
**Version**: 2.1  
**Status**: Production Ready  
**Author**: SSO Portal Development Team
-