import os

def contains_cs_file(path):
    # Check if a directory contains any .cs files
    for root, dirs, files in os.walk(path):
        for file in files:
            if file.endswith('.cs'):
                return True
    return False

def generate_folder_structure(start_path='.'):
    folder_structure = []

    for root, dirs, files in os.walk(start_path):
        # Skip directories that do not contain .cs files
        if not contains_cs_file(root):
            continue
        
        level = root.replace(start_path, '').count(os.sep)
        indent = '    ' * level
        folder_structure.append(f'{indent}{os.path.basename(root)}/')
        subindent = '    ' * (level + 1)
        
        for f in files:
            if f.endswith('.cs'):
                folder_structure.append(f'{subindent}{f}')

    return folder_structure

def save_structure_to_file(filename, structure):
    with open(filename, 'w') as f:
        for line in structure:
            f.write(line + '\n')

if __name__ == '__main__':
    structure = generate_folder_structure()
    save_structure_to_file('folder_structure.txt', structure)
    print("Folder structure saved to 'folder_structure.txt'")
