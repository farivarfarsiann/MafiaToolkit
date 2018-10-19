#include "M2Model.h"
#include "Utilities.h"

//===================================================
//		ModelPart
//===================================================
void ModelPart::SetHasPositions(bool b) {
	ModelPart::hasPosition = b;
}

void ModelPart::SetHasNormals(bool b) {
	ModelPart::hasNormals = b;
}

void ModelPart::SetHasTangents(bool b) {
	ModelPart::hasTangents = b;
}

void ModelPart::SetHasBlendData(bool b) {
	ModelPart::hasBlendData = b;
}

void ModelPart::SetHasFlag0x80(bool b) {
	ModelPart::hasFlag0x80 = b;
}

void ModelPart::SetHasUV0(bool b) {
	ModelPart::hasUV0 = b;
}

void ModelPart::SetHasUV1(bool b) {
	ModelPart::hasUV1 = b;
}

void ModelPart::SetHasUV2(bool b) {
	ModelPart::hasUV2 = b;
}

void ModelPart::SetHasUV7(bool b) {
	ModelPart::hasUV7 = b;
}

void ModelPart::SetHasFlag0x20000(bool b) {
	ModelPart::hasFlag0x20000 = b;
}

void ModelPart::SetHasFlag0x40000(bool b) {
	ModelPart::hasFlag0x40000 = b;
}

void ModelPart::SetHasDamage(bool b) {
	ModelPart::hasDamageGroup = b;
}

void ModelPart::SetVertSize(int count) {
	ModelPart::vertSize = count;
}

void ModelPart::SetVertices(std::vector<Point3> vertices, bool updateCount) {
	ModelPart::vertices = vertices;

	if(updateCount)
		ModelPart::vertSize = vertices.size();
}

void ModelPart::SetNormals(std::vector<Point3> normals) {
	ModelPart::normals = normals;
}

void ModelPart::SetTangents(std::vector<Point3> tangents) {
	ModelPart::tangents = tangents;
}

void ModelPart::SetUVs(std::vector<UVVert> uvs) {
	ModelPart::uvs = uvs;
}

void ModelPart::SetSubMeshCount(int count) {
	ModelPart::subMeshCount = count;
}

void ModelPart::SetMatNames(std::vector<std::string> names, bool updateCount) {
	ModelPart::matNames = names;

	if (updateCount)
		ModelPart::subMeshCount = names.size();
}

void ModelPart::SetIndicesSize(int count) {
	ModelPart::indicesSize = count;
}

void ModelPart::SetIndices(std::vector<Int3> indices, bool updateCount) {
	ModelPart::indices = indices;

	if (updateCount)
		ModelPart::indicesSize = indices.size();
}

void ModelPart::SetMatIDs(std::vector<char> ids) {
	ModelPart::matIDs = ids;
}

bool ModelPart::GetHasPositions() {
	return ModelPart::hasPosition;
}

bool ModelPart::GetHasNormals() {
	return ModelPart::hasNormals;
}

bool ModelPart::GetHasTangents() {
	return ModelPart::hasTangents;
}

bool ModelPart::GetHasBlendData() {
	return ModelPart::hasBlendData;
}

bool ModelPart::GetHasFlag0x80() {
	return ModelPart::hasFlag0x80;
}

bool ModelPart::GetHasUV0() {
	return ModelPart::hasUV0;
}

bool ModelPart::GetHasUV1() {
	return ModelPart::hasUV1;
}

bool ModelPart::GetHasUV2() {
	return ModelPart::hasUV2;
}

bool ModelPart::GetHasUV7() {
	return ModelPart::hasUV7;
}

bool ModelPart::GetHasFlag0x20000() {
	return ModelPart::hasFlag0x20000;
}

bool ModelPart::GetHasFlag0x40000() {
	return ModelPart::hasFlag0x40000;
}

bool ModelPart::GetHasDamage() {
	return ModelPart::hasDamageGroup;
}

int ModelPart::GetVertSize() {
	return ModelPart::vertSize;
}

std::vector<Point3> ModelPart::GetVertices() {
	return ModelPart::vertices;
}

std::vector<Point3> ModelPart::GetNormals() {
	return ModelPart::normals;
}

std::vector<Point3> ModelPart::GetTangents() {
	return ModelPart::tangents;
}

std::vector<UVVert> ModelPart::GetUVs() {
	return ModelPart::uvs;
}

int ModelPart::GetSubMeshCount() {
	return ModelPart::subMeshCount;
}

int ModelPart::GetIndicesSize() {
	return ModelPart::indicesSize;
}

std::vector<Int3> ModelPart::GetIndices() {
	return ModelPart::indices;
}

std::vector<std::string> ModelPart::GetMatNames() {
	return ModelPart::matNames;
}

std::vector<char> ModelPart::GetMatIDs() {
	return ModelPart::matIDs;
}

void ModelPart::ReadFromStream(FILE * stream) {
	fread(&hasPosition, sizeof(bool), 1, stream);
	fread(&hasNormals, sizeof(bool), 1, stream);
	fread(&hasTangents, sizeof(bool), 1, stream);
	fread(&hasBlendData, sizeof(bool), 1, stream);
	fread(&hasFlag0x80, sizeof(bool), 1, stream);
	fread(&hasUV0, sizeof(bool), 1, stream);
	fread(&hasUV1, sizeof(bool), 1, stream);
	fread(&hasUV2, sizeof(bool), 1, stream);
	fread(&hasUV7, sizeof(bool), 1, stream);
	fread(&hasFlag0x20000, sizeof(bool), 1, stream);
	fread(&hasFlag0x40000, sizeof(bool), 1, stream);
	fread(&hasDamageGroup, sizeof(bool), 1, stream);
	fread(&vertSize, sizeof(int), 1, stream);
	vertices = std::vector<Point3>(vertSize);

	if(hasNormals)
		normals = std::vector<Point3>(vertSize);

	if(hasTangents)
		tangents = std::vector<Point3>(vertSize);

	if(hasUV0)
		uvs = std::vector<UVVert>(vertSize);

	for (int i = 0; i != vertSize; i++) {
		if (hasPosition) {
			fread(&vertices[i].x, sizeof(float), 1, stream);
			fread(&vertices[i].y, sizeof(float), 1, stream);
			fread(&vertices[i].z, sizeof(float), 1, stream);
		}
		if (hasNormals) {
			fread(&normals[i].x, sizeof(float), 1, stream);
			fread(&normals[i].y, sizeof(float), 1, stream);
			fread(&normals[i].z, sizeof(float), 1, stream);
		}
		if (hasTangents) {
			fread(&tangents[i].x, sizeof(float), 1, stream);
			fread(&tangents[i].y, sizeof(float), 1, stream);
			fread(&tangents[i].z, sizeof(float), 1, stream);
		}
		if (hasUV0) {
			fread(&uvs[i].x, sizeof(float), 1, stream);
			fread(&uvs[i].y, sizeof(float), 1, stream);
		}
	}
	fread(&subMeshCount, sizeof(int), 1, stream);
	matNames = std::vector<std::string>(subMeshCount);
	for (int i = 0; i != subMeshCount; i++) {
		std::string edmName = std::string();
		edmName = ReadString(stream, edmName);
		matNames[i] = edmName;
	}
	fread(&indicesSize, sizeof(int), 1, stream);
	indices = std::vector<Int3>(indicesSize);
	matIDs = std::vector<char>(indicesSize);
	for (int x = 0; x != indicesSize; x++) {
		Int3 tri;
		char matID;
		fread(&tri.i1, sizeof(short), 1, stream);
		fread(&tri.i2, sizeof(short), 1, stream);
		fread(&tri.i3, sizeof(short), 1, stream);
		fread(&matIDs[x], sizeof(char), 1, stream);
		indices[x] = tri;
	}
}

void ModelPart::WriteToStream(FILE * stream) {
	fwrite(&hasPosition, sizeof(bool), 1, stream);
	fwrite(&hasNormals, sizeof(bool), 1, stream);
	fwrite(&hasTangents, sizeof(bool), 1, stream);
	fwrite(&hasBlendData, sizeof(bool), 1, stream);
	fwrite(&hasFlag0x80, sizeof(bool), 1, stream);
	fwrite(&hasUV0, sizeof(bool), 1, stream);
	fwrite(&hasUV1, sizeof(bool), 1, stream);
	fwrite(&hasUV2, sizeof(bool), 1, stream);
	fwrite(&hasUV7, sizeof(bool), 1, stream);
	fwrite(&hasFlag0x20000, sizeof(bool), 1, stream);
	fwrite(&hasFlag0x40000, sizeof(bool), 1, stream);
	fwrite(&hasDamageGroup, sizeof(bool), 1, stream);
	fwrite(&vertSize, sizeof(int), 1, stream);

	for (int i = 0; i != vertSize; i++) {
		if (hasPosition) {
			fwrite(&vertices[i].x, sizeof(float), 1, stream);
			fwrite(&vertices[i].y, sizeof(float), 1, stream);
			fwrite(&vertices[i].z, sizeof(float), 1, stream);
		}
		if (hasNormals) {
			fwrite(&normals[i].x, sizeof(float), 1, stream);
			fwrite(&normals[i].y, sizeof(float), 1, stream);
			fwrite(&normals[i].z, sizeof(float), 1, stream);
		}
		if (hasTangents) {
			fwrite(&tangents[i].x, sizeof(float), 1, stream);
			fwrite(&tangents[i].y, sizeof(float), 1, stream);
			fwrite(&tangents[i].z, sizeof(float), 1, stream);
		}
		if (hasUV0) {
			fwrite(&uvs[i].x, sizeof(float), 1, stream);
			fwrite(&uvs[i].y, sizeof(float), 1, stream);
		}
	}
	fwrite(&subMeshCount, sizeof(int), 1, stream);

	for (int i = 0; i != subMeshCount; i++)
		WriteString(stream, matNames[i]);

	fwrite(&indicesSize, sizeof(int), 1, stream);
	for (int i = 0; i != indices.size(); i++) {
		fwrite(&indices[i].i1, sizeof(short), 1, stream);
		fwrite(&indices[i].i2, sizeof(short), 1, stream);
		fwrite(&indices[i].i3, sizeof(short), 1, stream);
		fwrite(&matIDs[i], sizeof(char), 1, stream);
	}
}

ModelPart::ModelPart() {}
ModelPart::~ModelPart() {}

//===================================================
//		ModelStructure
//===================================================
void ModelStructure::SetName(std::string name) {
	ModelStructure::name = name;
}

void ModelStructure::SetPartSize(char count) {
	ModelStructure::partSize = count;
}

void ModelStructure::SetParts(std::vector<ModelPart> parts, bool updateCount) {
	ModelStructure::parts = parts;

	if (updateCount)
		ModelStructure::partSize = parts.size();
}

std::string ModelStructure::GetName() {
	return name;
}

char ModelStructure::GetPartSize() {
	return partSize;
}

std::vector<ModelPart> ModelStructure::GetParts() {
	return parts;
}

void ModelStructure::ReadFromStream(FILE * stream) {
	int header;
	fread(&header, sizeof(int), 1, stream); //header
	std::string edmName = std::string();
	edmName = ReadString(stream, edmName);
	name = edmName;
	fread(&partSize, 1, 1, stream);
	parts = std::vector<ModelPart>(partSize);

	for (int i = 0; i != parts.size(); i++)
		parts[i].ReadFromStream(stream);

	fclose(stream);
}

void ModelStructure::WriteToStream(FILE * stream) {
	int header = 542388813;
	fwrite(&header, sizeof(int), 1, stream);
	WriteString(stream, name);
	fwrite(&partSize, sizeof(char), 1, stream);

	for (int x = 0; x != parts.size(); x++)
		parts[x].WriteToStream(stream);

	fclose(stream);

}

ModelStructure::ModelStructure() {}
ModelStructure::~ModelStructure() {}