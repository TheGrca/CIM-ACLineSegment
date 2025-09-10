# Power System CIM Model Application

## Overview

This project involves modeling a power system structure based on the CIM (Common Information Model) standard. The application includes a WPF-based GUI, CIM/XML manipulation, GDA (Generic Data Access) integration, and the use of generated code based on an RDFS-exported CIM profile.

![UML Diagram](diagram.png)

## Technologies Used

- **WPF** (Windows Presentation Foundation)
- **C#** (.NET Framework 4.8)
- **CIM/XML & CIM Profiles**
- **RDFS, XMI**
- **GDA** (Generic Data Access) architecture
- **Enterprise Architect Viewer** (for diagram inspection)
- **CIMET tool** (for XML snippet generation)
- **Visual Studio** (.NET)

## Features

### 1. Get Values
- Retrieve specific entity values by type and position
- Support for multiple entity types:
  - PerLengthPhaseImpedance
  - PhaseImpedanceData
  - PerLengthSequenceImpedance
  - ACLineSegment
  - DCLineSegment
  - SeriesCompensator
  - Terminal

### 2. Get Extent Values
- Retrieve all entities of a specific type
- Dynamic attribute selection based on entity type
- Configurable property filtering

### 3. Get Related Values
- Navigate relationships between entities
- Support for forward relationship traversal
- Dynamic property and target type selection based on source entity

### 4. CIM/XML Import
- Import CIM/XML model files
- Convert CIM models to internal Delta format
- Apply model changes to the network model service
